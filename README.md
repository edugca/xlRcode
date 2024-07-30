# xlRcode (v. 0.2.0)
<img src="[https://github.com/favicon.ico](https://github.com/user-attachments/assets/4039bd04-d312-46f3-b0bd-480658f983c5)" width="100">

Call R from Excel. Create new Excel functions that make use of R packages. Integrate both tools seamlessly.

A work in progress...

## Documentation

For documentation, check the spreadsheet file "xlRcode - Main.xlsx".			
	
New Excel functions				
				
- **XLRCODE**
  - Run R code in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.
- **XLRCODE_ENV**
  - Run R code in a pre-specified environment and transfer values from and to Excel.
- **XLRSCRIPT**
  - Run R script in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.
- **XLRSCRIPT_ENV**
  - Run R script in a pre-specified environment and transfer values from and to Excel.
- **XLRFUNC**
  - Call an R function in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.
- **XLRFUNC_ENV**
  - Call an R function in a pre-specified environment and transfer values from and to Excel.
- **XLRDATE**
  - Convert Excel dates to R dates or vice-versa.

## Requirements

It only works with Windows.

This add-in has been tested on Microsoft Excel 365 64-bit. It may work with other versions of Excel for Windows, but it is not guaranteed.

You are going to need that R is installed on your machine. You can download an installer from here: https://cran.r-project.org/index.html.

This add-in was tested with R v. 4.0.2. It may work with other versions, though.

If you already have R installed on your machine, you may wish to make an independent installation just for this add-in. It is up to you.

## How to install for the first time?

1) On this webpage, download the ZIP file with the latest version of xlRcode.
2) Find on your computer the zip file you have just downloaded.
3) Extract all files in the Release subfolder of the zip file to a folder on your computer. This is where the add-in will be installed.
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
4) On this webpage, download the ZIP file with the latest version of xlRcode.
5) Find on your computer the zip file you have just downloaded.
6) Extract all files in the zip file to the same folder in your computer you had previously installed the add-in.
7) If step-2 advice applies to you, this is the time to bring the backup scripts back to their original folder.
8) Open Excel.
9) In the xlRcode ribbon tab, click on the "About" button and check whether the version displayed matches the version just installed.

That should do the trick!

## Getting up to speed

# TIPs

- In general, tips for making R code faster also apply to xlRcode. You can find some tips in the following links:
https://www.dartistics.com/fast-r-code.html
https://bookdown.org/content/d1e53ac9-28ce-472f-bc2c-f499f18264a3/speedtips.html
- Use array formulas whenever possible as they reduce the amount of individual calls from Excel to R and therefore each function overhead time toll.
- Time-demanding commands/scripts should be called with care from cells because the R status while running code may be opaque.
- You can split commands in different cells and provide the reference to their joint address as the code argument in XLRCODE or XLRCODE_ENV functions. Cells will be read in the same environment in order from left to right and then from top to bottom.
- You can split script paths in different cells and provide the reference to their joint address as the code argument in XLRSCRIPT or XLRSCRIPT_ENV functions. Cells will be read in the same environment in order from left to right and then from top to bottom.

## History

### v. 0.2.0
Major improvement and bug handling.
- Major improvement: all XLRCODE functions are now asynchronous!
- Major improvement: all XLRCODE functions run in an exclusive thread with stack size of 4Mb. This solves stack overflow exceptions that were raised when loading some popular packages such as 'devtools'.
- Improvement: functions now distinguish between NA and NAN, so that they return #N/A! and #NUM!, respectively.
- Improvement: functions XLRCODE and XLRCODE_ENV now read commands in multiple cells in order.
- Improvement: functions XLRCODE, XLRCODE_ENV, XLRFUNC, and XLRFUNC_ENV now read parameter names and values listed in multiple cells.
- Improvement: functions XLRSCRIPT and XLRSCRIPT_ENV now read script paths in multiple cells in order.
- Improvement: search mechanism in the form of installed packages is now case insensitive by default, but allows case sensitive searches as well.
- Improvement: functions that return NULL to Excel are displayed as #NULL!.
- Improvement: R commands with no return value, such as comments, now return an empty string when called from XLRCODE functions.
- Improvement: Packages form now exhibits a description for each package installed.
- Improvement: Packages form now a tab of "Old Packages" that shows for which packages there are updates available on the CRAN server.
- Improvement: ExcelDNA logging is now redirected to an external file.
- Improvement: Calculate range button on the ribbon now allows for recalculation of the entire active worksheet or the entire active workbook.
- Improvement: a new group of buttons to refresh pictures was added.
- Improvement: installing/uninstalling and loading packages now specify their library.
- Bug fix: function XLRDATE was not successfully converting Excel dates to R ones because the latter were being reconverted to Excel format before returning to the spreadsheet.
- Bug fix: when one tried to open R console as administrator but process failed, one would have to restart Excel to try it again.
- Bug fix: when XLRDATE was applied to a two-dimensional matrix of Excel dates it was returning all values stacked up in a single vector.
- Bug fix: error was raised when a XLRCODE function was called before the Console form was displayed and "Register Excel function calls" was enabled.
- Bug fix: XLRCODE_ENV and XLRFUNC_ENV functions raised error when "Environment" argument contained invalid characters, such as " " and "=".
- Bug fix: XLRCODE and XLRCODE_ENV functions raised error when the "commands" argument was a range of cells that included an empty cell. Now, it ignores empty cells.
- Bug fix: when user passed an invalid environment argument to XLRCODE functions, Excel crashed.
- Bug fix: some forms and error messages were being displayed for the first time in a different monitor than the one currently occupied by the Excel app.
- Bug fix: R console only displayed lists whose elements had names.
- Bug fix: code arguments in XLRCODE functions that included cells with comments were raising error.

### v. 0.1.9
Minor improvement and bug handling.
- Improvement: ExcelDna was updated to version 1.6.1-beta3.
- Improvement: when user opens R terminal through ribbon, xlRcode now asks whether admin privileges should be requested.
- Improvement: dataframe datatypes "dfh", "dfc", and "dfr" now check whether dataframe content is only numerical or not.
- Improvement: R code error messages returned from XLRCODE functions are displayed as in R.
- Improvement: "Graphic" button on the ribbon now brings to front current X11 graphic device in case there is one open.
- Bug fix: String outputs were being parsed from UTF-8 (R's default encoding) to UNICODE without due conversion.
- Bug fix: R console messages were duplicated in case of error.

### v. 0.1.8
Minor improvement and bug handling.
- Improvement: xlRcode is still not passing through some anti-virus detectors due to a false alert raised for ExcelDna XLL files. ExcelDna was updated to version 1.6.1-beta1. Problem persists, though.
- Improvement: Matrix arguments with mixed datatypes can now be parsed by xlRcode functions. Each element is converted to the string datatype as in R.
- Bug fix: String arguments were being parsed from UNICODE to UTF-8 (R's default encoding) without due conversion.
- Bug fix: Function XLRCODE_RTD still under development was visible.
- Bug fix: Logical matrix arguments were not being parsed by xlRcode functions.

### v. 0.1.7
Minor improvement and bug handling.
- Improvement: xlRcode is still not passing through some anti-virus detectors due to a false alert raised for ExcelDna XLL files. ExcelDna was updated to version 1.6.1-alpha3. Problem persists, though.
- Bug fix: Empty cells and Excel error values in numerical dataframes were raising errors or being wrongly parsed. Now, they are parsed as NaN.
- Bug fix: Reload Scripts' message box was being displayed behind other forms.

### v. 0.1.6
Minor improvement and bug handling.
- Improvement: xlRcode is still not passing through some anti-virus detectors due to a false alert raised for ExcelDna XLL files. ExcelDna was updated to version 1.6.1-alpha2. Problem persists, but for a lower amount of anti-virus programs.
- Improvement: XLRCODE functions now accept the argument suffix "(asis)", which parses a string as literal code. For example, this allows parsing a funcion code as a function object.
- Improvement: visible output of R commands invoked from the console were being suppressed as the internal function XLRCODE_Routine was running in a different thread.
- Improvement: code console execution is stopped if user presses ESC.
- Bug fix: an error was raised when an already visible form was asked to be showed again.
- Bug fix: xlRcode functions which have an environment parameter raised an unhandled error when environment name was invalid.
- Bug fix: code console intellisense raised untreated error if R engine was not working.

### v. 0.1.5
Minor improvement.
- Improvement: xlRcode was not passing through some anti-virus detectors due to a false alert raised for ExcelDna XLL files. Packing was disabled for it is unnecessary and ExcelDna was updated to version 1.6.0-rc1.

### v. 0.1.4
Minor improvements and bug handling.
- Improvement: xlRcode functions have now full descriptions, including their arguments, in the Function Wizard.
- Improvement: xlRcode functions have now intellisense.
- Improvement: user can specify a packages library folder at the SetUp form.
- Bug fix: arguments of functions XLRCODE, XLRFUNC and XLRSCRIPT were being processed in the global environment instead of a temporary one.
- Bug fix: non-modal forms were being exhibited at the top of all windows, including those outside Excel. Now, they are exhibited at the top of Excel.
- Bug fix: listing Environment objects when object was boolean raised untreated error.

### v. 0.1.3
Bug handling.
- Bug fix: boolean arguments in the xlRcode functions were not being correctly parsed from Excel to R.
- Bug fix: strict type parsing of arguments in "Make it Lambda" functions was not working and it was disabled for now.
- Bug fix: last optional argument of xlRcode functions was not being transferred from Excel to R.

### v. 0.1.2
Major change, minor improvements and bug handling.
- Major change: functions XLRFUNCTION and XLRFUNCTION_ENV were renamed to XLRFUNC and XLRFUNC_ENV, respectively;
- Improvement: functions XLRFUNCTION and XLRFUNCTION_ENV now accept datatype coercion suffixes in their parameter names.
- Improvement: the R datatype Sys.Date is now recognized and automatically converted to Excel dates when it is the output of a xlRcode function.
- Improvement: the R datatype Sys.Date is now displayed in the format 'YYYY-mm-dd' in the console. It was being displayed as an integer.
- Improvement: new function XLRDATE allows the user to convert dates from Excel to R and vice-versa.
- Improvement: new functionality allows the user to automatically convert R functions into Excel Lambda ones; the button "Make it Lambda" was added to the Ribbon tab.
- Improvement: functions will not recalculate if the Function Wizard is open. This avoids long waitings and freezing in case of very slow calculations.
- Improvement: registering/logging of Excel calls to xlRcode is optional now. It was added a checkbox to the SetUp form to control this. When disabled (default), it should improve speed.
- Improvement: the button "Reload scripts" was added to the Ribbon tab.
- Improvement: when button "Calculate range" is pressed, a message is displayed at Excel's Status Bar while calculating.
- Improvement: added an informative handler for unhandled exceptions with ExcelDna (ExcelIntegration.RegisterUnhandledExceptionHandler).
- Bug fix: loading scripts from the Scripts folder with syntax errors or dependency of resources in uninstalled/unloaded packages was raising untreated error. Informative message was implemented.
- Bug fix: calling xlRcode functions and leaving the argument of the last parameter missing was making that parameter to be ignored instead of treated as missing.
- Bug fix: listing Environment objects when there was a missing object raised untreated error.
- Bug fix: script files in subfolders of the Scripts folder were being ignored during start-up.

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
