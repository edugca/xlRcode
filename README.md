# xlRcode v. 0.1

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

## How to install?

1) At the top-right corner of this webpage, click on "Code" and then on "Download ZIP".
2) Find on your computer the zip file you have just downloaded.
3) Extract all files in the zip file to a folder on your computer. This is where the add-in will be installed.
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
13) Scripts folder could be any folder, but I strongly advise you either to select the subfolder "R Scripts" located inside the folder you installed xlRcode or copy the scripts located there to your scripts folder.
14) You will not be able to pick a packages repository until you restart Excel. Later on, feel free to do it.
15) Click on the "Save" button.
16) Restart Excel!

That should do the trick!

## History

### v. 0.1
First beta release.

=======
