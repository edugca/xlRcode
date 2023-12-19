# New Excel functions

<table><tbody><tr><td><strong>XLRCODE</strong></td><td>Run R code in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.</td></tr><tr><td><strong>XLRCODE_ENV</strong></td><td>Run R code in a pre-specified environment and transfer values from and to Excel</td></tr><tr><td><strong>XLRSCRIPT</strong></td><td>Run R script in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.</td></tr><tr><td><strong>XLRSCRIPT_ENV</strong></td><td>Run R script in a pre-specified environment and transfer values from and to Excel</td></tr><tr><td><strong>XLRFUNC</strong></td><td>Call an R function in a local environment and transfer values from and to Excel</td></tr><tr><td><strong>XLRFUNC_ENV</strong></td><td>Call an R function in a pre-specified environment and transfer values from and to Excel</td></tr></tbody></table>

## XLRCODE

Run R code in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.

<table><tbody><tr><td>Return</td><td>Function returns the visible result of the last command of the code. In case it is not&nbsp; visible, function returns TRUE.</td></tr><tr><td>Parameters</td><td>Description</td></tr><tr><td>Commands</td><td>R code</td></tr><tr><td>[ParamsList]...</td><td>Sequence of optional paired parameters, where the first parameter is a valid variable name (as a text string) and the second parameter is its value. Variable names with the (optional) following suffixes are coerced to specific data types: (n) =&gt; double, (s) =&gt; string, (d) =&gt; date, (df) =&gt; data frame. Other coercions are available at the "Data types" worksheet.</td></tr></tbody></table>

### Example: Concatenate two strings placing a separator in-between them

![image](https://user-images.githubusercontent.com/28639516/223566506-6e270823-ee87-4b82-ba2c-beec22ed2f27.png)

## XLRCODE\_ENV

Run R code in a pre-specified environment and transfer values from and to Excel.

<table><tbody><tr><td>Return</td><td>Function returns the visible result of the last command of the code. In case it is not&nbsp; visible, function returns TRUE.</td></tr><tr><td>Parameters</td><td>Description</td></tr><tr><td>[Environment]</td><td>Name of the environment in which to run the R code. If omitted or empty, code is run inside Global environment.</td></tr><tr><td>Commands</td><td>R code</td></tr><tr><td>[ParamsList]...</td><td>Sequence of optional paired parameters, where the first parameter is a valid variable name (as a text string) and the second parameter is its value. Variable names with the (optional) following suffixes are coerced to specific data types: (n) =&gt; double, (s) =&gt; string, (d) =&gt; date, (df) =&gt; data frame. Other coercions are available at the "Data types" worksheet.</td></tr></tbody></table>

### Example: Concatenate two strings placing a separator in-between them

![image](https://user-images.githubusercontent.com/28639516/223569743-adf7b2ea-2761-4117-832d-e32e89e0efa5.png)

## XLRSCRIPT

Run R script in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.

<table><tbody><tr><td>Return</td><td>Function returns the visible result of the last command of the script. In case it is not&nbsp; visible, function returns TRUE.</td></tr><tr><td>Parameters</td><td>Description</td></tr><tr><td>ScriptPath</td><td>R script file path</td></tr><tr><td>[ParamsList]...</td><td>Sequence of optional paired parameters, where the first parameter is a valid variable name (as a text string) and the second parameter is its value. Variable names with the (optional) following suffixes are coerced to specific data types: (n) =&gt; double, (s) =&gt; string, (d) =&gt; date, (df) =&gt; data frame. Other coercions are available at the "Data types" worksheet.</td></tr></tbody></table>

### Example: Concatenate two strings placing a separator in-between them

![](https://user-images.githubusercontent.com/28639516/223570774-b3c713bd-dadd-4222-84d5-c21930969ac4.png)

![](https://user-images.githubusercontent.com/28639516/223570297-28bb4e3b-7fac-41ad-b6e7-eb29882e6168.png)

## XLRSCRIPT\_ENV

Run R script in a pre-specified environment and transfer values from and to Excel.

<table><tbody><tr><td>Return</td><td>Function returns the visible result of the last command of the script. In case it is not&nbsp; visible, function returns TRUE.</td></tr><tr><td>Parameters</td><td>Description</td></tr><tr><td>[Environment]</td><td>Name of the environment in which to run the R code. If omitted or empty, code is run inside Global environment.</td></tr><tr><td>ScriptPath</td><td>R script file path</td></tr><tr><td>[ParamsList]...</td><td>Sequence of optional paired parameters, where the first parameter is a valid variable name (as a text string) and the second parameter is its value. Variable names with the (optional) following suffixes are coerced to specific data types: (n) =&gt; double, (s) =&gt; string, (d) =&gt; date, (df) =&gt; data frame. Other coercions are available at the "Data types" worksheet.</td></tr></tbody></table>

### Example: Concatenate two strings placing a separator in-between them

![](https://user-images.githubusercontent.com/28639516/223570774-b3c713bd-dadd-4222-84d5-c21930969ac4.png)

![](https://user-images.githubusercontent.com/28639516/223571238-c9eb3b0e-ce3a-475f-9671-22586fdfd6c9.png)

## XLRFUNC

Call an R function in a local environment and transfer values from and to Excel. Function memory is cleared after calculation.

<table><tbody><tr><td>Return</td><td>Function returns the result of the R function called.</td></tr><tr><td>Parameters</td><td>Description</td></tr><tr><td>FunctionName</td><td>Name of the function to be called. Consider specifying the whole function path to avoid confusion.</td></tr><tr><td>[AfterFunction]</td><td>In case the function called returns a complex structure and one wants to access a nested element, this parameter value is appended to the result object, i.e. [1]</td></tr><tr><td>[ParamsList]...</td><td>Sequence of optional paired parameters, where the first parameter is a valid variable name (as a text string) and the second parameter is its value. In case one wants to list arguments in the default order received by the function, without identifying them, provide "..." as a parameter followed by the desired arguments. Mixing identified and non-identified arguments is possible as long as all non-identified parameters come at the end. Moreover, one can pass a string with the literal specification of parameter names and their respective arguments as in R code. For that, one should pass "*" as the =XLRFUNC parameter name and the literal R expression as its value. Variable names with the (optional) following suffixes are coerced to specific data types: (n) =&gt; double, (s) =&gt; string, (d) =&gt; date, (df) =&gt; data frame. Other coercions are available at the "Data types" worksheet.</td></tr></tbody></table>

### Example: Concatenate two strings placing a separator in-between them

![](https://user-images.githubusercontent.com/28639516/223571608-9245b6fe-5cdb-4e27-991e-b2039fd5621f.png)

## XLRFUNC\_ENV

Call an R function in a pre-specified environment and transfer values from and to Excel.

<table><tbody><tr><td>Return</td><td>Function returns the result of the R function called.</td></tr><tr><td>Parameters</td><td>Description</td></tr><tr><td>[Environment]</td><td>Name of the environment in which to call the R function. If omitted or empty, function is called inside Global environment.</td></tr><tr><td>FunctionName</td><td>Name of the function to be called. Consider specifying the whole function path to avoid confusion.</td></tr><tr><td>[AfterFunction]</td><td>In case the function called returns a complex structure and one wants to access a nested element, this parameter value is appended to the result object, i.e. [1]</td></tr><tr><td>[ParamsList]...</td><td>Sequence of optional paired parameters, where the first parameter is a valid variable name (as a text string) and the second parameter is its value. In case one wants to list arguments in the default order received by the function, without identifying them, provide "..." as a parameter followed by the desired arguments. Mixing identified and non-identified arguments is possible as long as all non-identified parameters come at the end. Moreover, one can pass a string with the literal specification of parameter names and their respective arguments as in R code. For that, one should pass "*" as the XLRFUNC parameter name and the literal R expression as its value. Variable names with the (optional) following suffixes are coerced to specific data types: (n) =&gt; double, (s) =&gt; string, (d) =&gt; date, (df) =&gt; data frame. Other coercions are available at the "Data types" worksheet.</td></tr></tbody></table>

### Example: Concatenate two strings placing a separator in-between them

![](https://user-images.githubusercontent.com/28639516/223571842-8a49ef1e-bc3c-4352-8bb4-b65c328c3b99.png)

## XLRDATE

*   Convert Excel dates to R dates or vice-versa.
