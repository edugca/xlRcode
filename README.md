# xlRcode v. 0.1.1

Call R from Excel. Create new Excel functions that make use of R packages. Integrate both tools seamlessly.

A work in progress...

## Documentation

For documentation, check the spreadsheet file "xlRcode - Main.xlsx".			
	
New Excel functions				
				
- **XLRCODE**
  - Run R code in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.
- **XLRCODE_ENV**
  - Run R code in a pre-specified environment and transfer values from and to Excel
- **XLRSCRIPT**
  - Run R script in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.
- **XLRSCRIPT_ENV**
  - Run R script in a pre-specified environment and transfer values from and to Excel
- **XLRFUNCTION**
  - Call an R function in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.
- **XLRFUNCTION_ENV**
  - Call an R function in a pre-specified environment and transfer values from and to Excel

## Requirements

It only works with Windows.

This add-in has been tested on Microsoft Excel 365 64-bit. It may work with other versions of Excel for Windows, but it is not guaranteed.

You are going to need that R is installed on your machine. You can download an installer from here: https://cran.r-project.org/index.html.

This add-in was tested with R v. 4.0.2. It may work with other versions, though.

If you already have R installed on your machine, you may wish to make an independent installation just for this add-in. It is up to you.

## How to install for the first time?

1) At the top-right corner of this webpage, click on "Code" and then on "Download ZIP".
2) Find on your computer the zip file you have just downloaded.
3) Extract all files in Release subfolder of the zip file to a folder on your computer. This is where the add-in will be installed.
4) Open Excel. Go to File / Options / Add-ins. Then on the drop-down named "Manage", select "Excel Add-ins" and click on "Go...".
5) It will pop-up a window named "Add-ins". Click on "Browse...".
6) Find the folder you have created for installing the add-in. From there access the subfolder "Release" and select the file xlRcode-AddIn64.xll. If your Excel is 32-bits, you should select xlRcode-AddIn.xll instead. Click on the "Open" button.
7) Back to the "Add-ins" window, click on the "OK" button.

Whenever you open Excel, the add-in should be automatically loaded now!

8) In the Ribbon, you should see a new tab named "xlRcode". Click on it.
9) Click on the button "SetUp". A window named "xlRcode SetUp" will pop-up.
10) You will need to set the paths for the folders of your R installation.
11) R Home should be equivalent to "C:\Program Files\R\R-x.x.x", where x.x.x stands for the version of your R installation.
12) R Path should be equivalent to "C:\Program Files\R\R-x.x.x\bin\yyy", where x.x.x stands for the version of your R installation and yyy is either x64 or i386. If your Excel is 32-bits, you should choose i386.
13) Scripts folder could be any folder, but I strongly advise you to create a new folder and copy to it the scripts of the "R Scripts" subfolder located inside the folder you installed xlRcode.
14) You will not be able to pick a packages repository until you restart Excel. Later on, feel free to do it.
15) Click on the "Save" button.
16) Restart Excel!

That should do the trick!

## How to update from a previous version?

1) Close Excel.
2) If, by any chance, your Scripts folder is inside the folder you previously installed this addin and you have added or made changes to any of its scripts, make a backup copy of your Scripts folder.
3) Go to the folder you previously installed this addin and delete all of its contents.
4) At the top-right corner of this webpage, click on "Code" and then on "Download ZIP".
5) Find on your computer the zip file you have just downloaded.
6) Extract all files in the zip file to the same folder in your computer you had previously installed the add-in.
7) If step-2 advice applies to you, this is the time to bring the backup scripts back to their original folder.
8) Open Excel.
9) In the xlRcode ribbon tab, click on the "About" button and check whether the version displayed matches the version just installed.

That should do the trick!

## History

### v. 0.1.1
Minor improvements and bug handling.
- Improvement: Set-Up form is now displayed above others.
- Improvement: the first time a form is opened, it is now located at the center of the screen of the Excel monitor (multiple monitors issue). Modal forms still remember their position before hiding.
- Bug fix: an error handler was added to the AutoOpen routine to obtain clearer info on start-up errors.
- Bug fix: trying to select an online packages repository was prompting untretaed error if R was not loaded.
- Bug fix: trying to open the Scripts folder from the Console form before it was set by the user prompted an error.

### v. 0.1
First beta release.

=======
