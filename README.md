## Data visualization 

All content in this repository is made for the Master Thesis of Jesper Bo Sembach and Niels Beuscahu, DTU 2017. This solution is specialized to extract relevant content from a file structure hidden from this repository. The solution is specialized and most of the work cannot be applied to other data without the need of heavy code-edits. 

### Contents
* ``Visualization`` (folder) - plots (box plots and graphs) of performance of different parameters (CPU, HDD, RAM..) and box plots to compare different measurements.
* ``Obsmon`` (folder) - files for each ransomware containing list of encrypted files and time of encryption sorted after time. 
* ``DataExtractNManipulation.ipynb`` - the code generating the plots in the Visualization folder.
  * ``ExtractNManipulation.py`` - the .py-version of the exact same code found in the iPython Notebook.
* ``ProofOfConcept.ipynb`` - some code snippets testing different pythonic methods.
* ``allExtensionsOnBaseline.txt`` - the set of the file extensions encrypted by each ransomware
  * ``allExtensionsOnBaseline.xlsx`` - the .xslx-version of the .txt file of extensions (more ledgible)
* ``BaselineFileData.txt`` - A list of all folders and files found and worked upon by each of the testing methods.  
