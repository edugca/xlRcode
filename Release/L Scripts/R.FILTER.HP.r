# This collection of functions provides access to FILTER methods
#
# Author: Eduardo G C Amaral
# Last update: September 11, 2022
#
# Use at your own risk

#[PACKAGES] mFilter
#[EXCEL_LAMBDA] R.FILTER.HP=LAMBDA([values],[trendOrCycle],[lambdaParameter],[hasDrift], XLRFUNC("xlRcode$Functions$R.FILTER.HP", , "values", values, "trendOrCycle", trendOrCycle, "lambdaParameter", lambdaParameter, "hasDrift", hasDrift))
xlRcode$Functions$R.FILTER.HP <- function(values, trendOrCycle = 'C', lambdaParameter = 14400, hasDrift = FALSE){

  # Load packages
  library(mFilter)

  # Treat arguments
  if( !any(toupper(trendOrCycle) , c('T', 'C', 'TREND', 'CYCLE') ) ){
    errorMsg = '# trendOrCycle must be "T", "C", "TREND" or "CYCLE"'
    return(errorMsg)
  }
  if( !is.numeric(lambdaParameter) ){
    errorMsg = '# Lambda must be numeric'
    return(errorMsg)
  }
  if( !is.logical(hasDrift) ){
    errorMsg = '# hasDrift must be TRUE or FALSE'
    return(errorMsg)
  }
  isColumn <- TRUE
  if( dim(values)[1] == 1 ){
	  isColumn <- FALSE
	  values <- matrix(values)
  }

  # Convert values to time series
  valuesTS <- ts(values)

  # Run filter
  filtered <- hpfilter(valuesTS, lambdaParameter, 'lambda', hasDrift)

  # Define return value
  if (toupper(trendOrCycle) == 'T' || toupper(trendOrCycle) == 'TREND'){
    final <- as.numeric(filtered$trend)
  } else if (toupper(trendOrCycle) == 'C' || toupper(trendOrCycle) == 'CYCLE'){
    final <- as.numeric(filtered$cycle)
  } else {
    errorMsg = '# trendOrCycle must be T (or TREND) or C (or CYCLE)'
    return(errorMsg)
  }

  # Transpose if needed
  if (isColumn){
	  final
  } else {
	  t(final)
  }

  #print(unemp.hp)
  #summary(unemp.hp)
  #residuals(unemp.hp)
  #fitted(unemp.hp)
  #plot(unemp.hp)

}