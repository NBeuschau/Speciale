# -------------------------------------------------------------------------------- #
# The Technical University of Denmark, June 2017
# Developer: Henriette Steenhoff, s134869
# Purpose:   Analysis and visualization of performance measures for Master Thesis of 
#            Jesper Bo Sembach and Niels Beuschau
# -------------------------------------------------------------------------------- #

# IMPORTS
import re
import json
# Only importing needed modules to avoid os.open being chosen instead of built-in open
from os import path, chdir, makedirs, listdir, getcwd 
import time
from datetime import datetime 
import pandas as pd
import numpy as np
import pylab as pl
import matplotlib as plt
%matplotlib inline

import plotly
import plotly.plotly as py
from IPython.display import Image 
import plotly.graph_objs as go
# API access to plotting tools
# Will need access to perform plotting
#plotly.tools.set_credentials_file(username=, api_key =)
plotly.tools.set_credentials_file(username='frksteenhoff2', api_key ='duu8hsfRmuI5rF2EU8o5')

basePath    = "C:/Users/frksteenhoff/Documents/GitHub/MSc-Sembach-Beuschau_Jun2017/"
pathToPlots = basePath + "Visualization"
obsmonPath  = basePath + "Obsmon"
# -------------------------------------------------------------------------------- #
# HELPER FUNCTIONS FOR THE VISUALIZATION SCRIPT BELOW
# -------------------------------------------------------------------------------- #

# Function creating correct layout style for each type of plot 
def setLayoutOptions(headTitle, ytitle):
    layout= go.Layout(
            title= headTitle.upper(),
            hovermode= 'closest',
            xaxis= dict(
                title= 'Time (0.5 second interval)',
                ticklen= 5,
                zeroline= True,
                gridwidth= 2,
            ),
            yaxis=dict(
                title= ytitle,
                ticklen= 5,
                gridwidth= 2,
            ),
            showlegend= False
    )
    return layout


# Remove all occurences of AppData paths from a given list of paths
def removeAppData(lst):
    outFile = []
    for f in lst:
        current_file = f.split('*')[0]
        pattern = re.compile('.*AppData.*')
        if not pattern.match(current_file):
            outFile.append(f)
    return outFile


# Split information from txt file in title and data
def splitTitleAndFiles(line, lineNumber):
    temp = []
    separteValues = []
    
    temp = line.strip('\n')
    titleAndList = temp.split(';')
    title = titleAndList[0]
        
    separateValues = titleAndList[1].split('?')
     
    if lineNumber in range(11,16):
        values = [val.replace(',','.') for val in separateValues]
    else:
        values = separateValues
    
    return title, values

    
# Create cpu, ram, hdd, handle and thread count plots
def createPerformancePlots(title, listValues, lineNumber, filename, folder, pathToPlots, pathToFiles):
    # NOTE! The plotting is done by using the plotly module.
    # The API connection might be lost and the function would need to be run anew. 
    mode = 'lines'
    
    # Set plotting marker
    # Commented out intentionally
    #if lineNumber == 11:
    #    mode = 'markers'
    #else:
    #    mode = 'lines'
        
    trace1 = go.Scatter(
        x = range(0,len(listValues)),
        y = listValues, 
        mode = mode,
        marker = dict(
            size = 3
        )
    )    
    
    # Choose right values for plotting cpu, ram or hdd, handle and thread count
    if lineNumber == 11:
        layout = setLayoutOptions(title, 'Percent')
    if lineNumber == 12:
        layout = setLayoutOptions(title, 'MB')
    elif lineNumber == 13:
        layout = setLayoutOptions(title, 'Speed - missing unit')
    elif lineNumber == 14:
        layout = setLayoutOptions(title, 'Number of handles')
    elif lineNumber == 15:
        layout = setLayoutOptions(title, 'Number of threads')
    else:    
        layout = setLayoutOptions(title, 'Byte')

    data = [trace1]
    fig  = go.Figure(data=data, layout=layout)
    
    # Create folder for plots if not exists
    dir_name = pathToPlots + "/" + folder
    if not path.exists(dir_name):
        makedirs(dir_name)
    chdir(dir_name)
    
    # Save image to file
    py.image.save_as(fig, filename=filename.strip('.txt') + "-"+ title + '.png')    
    chdir(pathToFiles)
    time.sleep(2) # Sleep added not to exhaust API

# Fetching file sizes from basline or test data
def getFileSizes(filename):
    sizeOfFiles = {}
    
    with open(filename, 'r') as f:
        paths = f.readlines()
    
    for lines in paths:
        values = lines.split('?')
        keyy = values[0].strip("C:\Users\Baseline\\")
        key_augmented = re.sub(r'\\+', '_', keyy)
        sizeOfFiles[key_augmented] = int(values[1])                                   
    return sizeOfFiles

                   
# Creating box plots and saving result to file
def createBoxplot(plottingValues, labels, filename, plotTitle, xlabel, ylabel, y_lim, titleOn):
    # Creating figure and setting plotting type
    pl.figure(figsize=(10,7))
    pl.boxplot(plottingValues)

    # Values - for ledgibility
    if titleOn:
        pl.title(plotTitle)
    pl.xlabel(xlabel)
    pl.ylabel(ylabel)
    pl.ylim([0,y_lim])
    # Label values
    pl.xticks(range(1,len(labels)+1), labels)
    chdir(pathToPlots)
    pl.savefig(filename + '.png')

def createStackedBarPlot2Values(data1, data2, labels, filename, color):
    n = len(labels)
    fig, ax = pl.subplots(figsize=(10,7))
    bar_locations = np.arange(n)
    ax.bar(bar_locations, data1)
    ax.bar(bar_locations, data2, bottom=data1, color=color)
    
    # Styling, labels and descriptions
    ax.set_xticks(np.arange(n) + 0.85 / 2)
    ax.set_xticklabels(labels)
    ax.legend(['Detection method reacted','No reaction'])
    pl.title('Detection success rate')
    pl.xlabel('Detection method')
    pl.ylabel('Frequency')
    
    chdir(pathToPlots)
    pl.savefig(filename + '.png')

# Get time between start and detection and detection and shutdown of ransomwares
def getTimeDeltas(startTime, detectionTime, shutdownTime):
    date_format = "%d-%m-%Y %H:%M:%S.%f"
    # Getting time values, creating date on correct date format
    started      = datetime.strptime(startTime[0], date_format)
    detected     = datetime.strptime(detectionTime[0], date_format)
    shutdown     = datetime.strptime(shutdownTime[0], date_format)
    # Calculate time from start -> detection and detection -> shutdown
    startDetTime = detected - started  
    detShutTime  = shutdown - detected
    
    # For testing purposes
    #print "start", started, 'detected', detected
    #print startDetTime
    return startDetTime, detShutTime


# Save obsmon observations for manual inspection
def saveObsmonToFile(data, filename, path):
    chdir(path)
    outfile = open(filename+'.txt', 'w')
    outfile.write("\n".join(data))

# Save lists to text file
def saveDataToFile(data, filename, path, join_criterion):
    chdir(path)
    outfile = open(filename+'.txt', 'a')
    outfile.write(join_criterion.join(data))

    
def saveSetOfFileExtensions(data, filename):
    setOfExtensions      = []
    fullListOfExtensions = []
    for ii in range(0,len(obsMon)):
        # Find all extensions for obsmon observations in baseline
        extension = obsMon[ii].split('*')[0].split('.')[-1]
        if not extension.lower().startswith('c:') and not extension.startswith(' ') and not extension.startswith('m\\') and not extension.startswith('20') :
            #print extension
            setOfExtensions.append(extension.lower())

    listToSave = sorted(set(setOfExtensions))
    listToSave[:0] = [files]
    return ", ".join(listToSave)


# -------------------------------------------------------------------------------- #
# The actual visualization script
# -------------------------------------------------------------------------------- #
# Basic inits
# Note! When creating boxlpots using line 3 and 20-22, the following slice is used: folders[1:]
# The code can be run in modules by adding the "and False" statement to the if-statements 
# the user wish to exclude. Below only the last if is run

folders = ['baseline', 'hp1', 'hp2','hp5', 'hp10', 'sh3', 'sh5', 'sh10', 'sh15']
successShutdown     = 0
lineNumber          = 0
first = True

boxplotDel         = [] # 2-d list for boxplot of deleted files
boxplotNew         = [] # 2-d list for boxplot of new files
obsMon             = [] # obsmon observations
fileSizes          = [] # list for file sizes (not implemented fully)
delLst             = [] # list of count of deleted files 
newLst             = [] # list of count of new files
failedShutdownCnt  = [] # list of number of failed shutdowns
successShutdownCnt = [] # list of shut downs
startDetLst        = [] # timedeltas from start to detection
detShutLst         = [] # timedeltas from detection to shutdown
boxStartDet        = [] # values for boxplot time from start to detection
boxDetShut         = [] # values for boxplot time from detection to shutdown
shutdownLst        = [] 
fullListOfExtensions = [] # list of ransomwares and related extensions

# Clear file extension list
outfile = open('allExtensionOnBaseline.txt', 'w')

chdir(basePath)
#baselineData = getFileSizes('BaselineFileData.txt') not needed

# For all folders in Speciale, access each folder and  'do something'
for folder in folders[1:]:
    pathToFiles   = basePath + "Speciale/" + folder 
    chdir(pathToFiles)
    selectedFiles = [file for file in listdir('.') if file.endswith('.txt')]
    print "Current folder: " + folder
    
    # For each file in current folder, read content 
    for files in selectedFiles:
        #print files
        # Read each line and split content into dictionary
        with open(files, 'r') as f:
            content = f.readlines()
        
        for lines in content:
            # Create list of values for boxplot of number of deleted files
            if lineNumber == 8 and lineNumber == -1:
                delFile, delCnt = splitTitleAndFiles(lines, lineNumber)
                delLst.append(int(delCnt[0]))
        
            # Create list of values for boxplot of number of new files
            elif lineNumber == 9 and lineNumber == -1:
                newFile, newCnt = splitTitleAndFiles(lines, lineNumber)
                newLst.append(int(newCnt[0]))
                
            # Data for cpu, ram, hdd, thread or handle count 
            elif lineNumber in range(11,16) and lineNumber == -1:
                # Get attribute and list of values
                title, listValues = splitTitleAndFiles(lines, lineNumber)
                    
                # Create plots cpu, ram, hdd, handle and thread count
                createPerformancePlots(title, listValues, lineNumber, files, folder, pathToPlots, pathToFiles)
               
            # fileMonObservations - the different files for analysis of order and file extensions
            # only for baseline, use slice folders[:1] when running, otherwise comment out!
            elif (lineNumber == 19 and lineNumber == -1):
                # Get attribute and list of values
                title, listValues = splitTitleAndFiles(lines, lineNumber)
                    
                # Extract all files not in folder AppData
                obsMon = removeAppData(listValues)
                #saveObsmonToFile(obsMon, files, obsmonPath)
                #chdir(pathToFiles)
            
                # save set of file extension in each obsmon list
                listToSave = saveSetOfFileExtensions(obsMon, files)
                
                fullListOfExtensions.append(listToSave)
                
            # Creating datastructures for boxplots of performance on shut-down
            elif (lineNumber == 3 or lineNumber ==  20 or lineNumber ==  21 or lineNumber ==  22):# and lineNumber == -1:
                # Create list of ransomwares in each test 
                # 1   detected, 
                # 0   not detected, 
                # '-' not included
                if folder == 'baseline' and first == True:
                    df = pd.DataFrame('-',index=selectedFiles, columns=folders)
                    del df['baseline'] # not needed, already on index
                    first = False
        
                # Start time
                if lineNumber == 3:
                    lineName, startTime = splitTitleAndFiles(lines, lineNumber)
                
                # Name of shutdown ransomware(s)
                elif lineNumber == 20:
                    lineName, shutdownLst = splitTitleAndFiles(lines, lineNumber)
                    
                # Time of detection
                elif lineNumber == 21:
                    lineName, detectionTime = splitTitleAndFiles(lines, lineNumber)                    
                    
                # Time of shutdown
                else:
                    lineName, shutdownTime = splitTitleAndFiles(lines, lineNumber)
                    
            ## Nothing implemented using sepcific information in file 
            #else:
            #    print ''#lines.strip('\n').split(';')[0].ljust(24), "nothing implemented"
            lineNumber += 1
        
        # Find number of ransomwares that do shut processes down.
        # '' indicates empty list
        if len(shutdownLst) >= 1 and shutdownLst[0] != '':
            successShutdown += 1
        #    df.loc[files][folder] = 1
        #else:
        #    df.loc[files][folder] = 0
            
            # Only calculate time if shutdown has occured
            if startTime[0] != '""' and detectionTime[0] != '""' and shutdownTime[0] != '""':
                # Only if startime is properly formatted
                # Calculate time from start -> detection and detection -> shutdown
                startDetTime, detShutTime = getTimeDeltas(startTime, detectionTime, shutdownTime)

                # Adding time differences to lists
            #    startDetLst.append(startDetTime.total_seconds())
                # If-statement for making the version taking hardware differences into account
                #if folder == 'hp5' or folder == 'hp10' or folder == 'sh15':
                #    detShutLst.append(detShutTime.total_seconds()*0.67)                    
                detShutLst.append(detShutTime.total_seconds())    
    
    
        # Reset variables
        lineNumber = 0
        fileSizes = []
        
    # Creating 2-d list for boxplots time
    #boxStartDet.append(startDetLst)
    boxDetShut.append(detShutLst)
    #startDetLst = []
    detShutLst  = []
        
    # Append to lists of fail/success for boxplot
    failedShutdownCnt.append(len(selectedFiles) - successShutdown)
    #successShutdownCnt.append(successShutdown)
    successShutdown = 0
    
    # Create 2-d list for boxplots - new and deleted files
    #boxplotDel.append(delLst)    
    #boxplotNew.append(newLst)
    #delLst = []
    #newLst = []

print "\nAll files read"

# Save set of extensions to file
saveDataToFile(fullListOfExtensions, 'allExtensionOnBaseline', basePath, "\n")

# Save dataframe to excel file of ransomware included/detection status
#writer = pd.ExcelWriter('ransomware_status.xlsx', engine='xlsxwriter')
#df.to_excel(writer, sheet_name='Ransomware_status')
#writer.save()

# Creating and saving boxplots for deleted and new files
#chdir(pathToPlots)
#createBoxplot(boxplotDel, folders, 'delFiles', 'Boxplot for deleted files', 'Distribution', 'Value', 3500, False)
#createBoxplot(boxplotNew, folders, 'newFiles', 'Boxplot for new files', 'Distribution', 'Value', 3500, False)
#print "Box plots created:"

# Creating and saving stacked bar plot for shutdown indication
#createStackedBarPlot2Values(successShutdownCnt, failedShutdownCnt, folders[1:], 'shutdown', 'r')

# Creating and saving box plot of time delta values 
# Start     -> detection
# Detection -> shutdown
#createBoxplot(boxStartDet, folders[1:], 'time_start-detection', 'Time from start to detection', 'Detection method', 'Seconds', 1000, True)
createBoxplot(boxDetShut, folders[1:], 'time_detection-shutdown_new', 'Time from detection to shutdown', 'Detection method', 'Seconds', 210, True)
