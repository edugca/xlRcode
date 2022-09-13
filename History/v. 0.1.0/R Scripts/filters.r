# This collection of functions provides access to FILTER methods
#
# Author: Eduardo G C Amaral
# Last update: September 10, 2022
#
# Use at your own risk


# Load R packages used by the scripts

local({
  
        # List of necessary packages to be installed from online repository
        #
        list.of.packages <- c("mFilter")
        new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        if(length(new.packages)) install.packages(new.packages)

})

##################################################
# FUNCTIONS EXPOSED TO EXCEL
##################################################

filterHP <- function(values, trendOrCycle = 'C', lambdaParameter = 14400, hasDrift = FALSE){

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

  # Convert values to time series
  valuesTS <- ts(values)

  # Run filter
  filtered <- hpfilter(valuesTS, lambdaParameter, 'lambda', hasDrift)

  # Define return value
  if (toupper(trendOrCycle) == 'T' || toupper(trendOrCycle) == 'TREND'){
    as.numeric(filtered$trend)
  } else if (toupper(trendOrCycle) == 'C' || toupper(trendOrCycle) == 'CYCLE'){
    as.numeric(filtered$cycle)
  } else {
    errorMsg = '# trendOrCycle must be T (or TREND) or C (or CYCLE)'
    return(errorMsg)
  }

  #print(unemp.hp)
  #summary(unemp.hp)
  #residuals(unemp.hp)
  #fitted(unemp.hp)
  #plot(unemp.hp)

}


##################################################
# UNDER CONSTRUCTION
##################################################

local({

filterTS <- function(values, trendOrCycle, filterName = 'HP', ...){

  # Load packages
  library(mFilter)

  valuesTS <- ts(values)

  if (toupper(filterName) == 'HP'){
    # Hodrick-Prescott
    filtered <- hpfilter(valuesTS)
  } else if (toupper(filterName) == 'BK'){
    # Baxter-King
    filtered <- bkfilter(valuesTS)
  } else if (toupper(filterName) == 'BW'){
    # Butterworth
    filtered <- bwfilter(valuesTS)
  } else if (toupper(filterName) == 'CF'){
    # Christiano-Fitzgerald
    filtered <- cffilter(valuesTS)
  } else if (toupper(filterName) == 'TR'){
    # Trigonometric regression
    filtered <- cffilter(valuesTS)
  }  else {
    errorMsg = '# Filter name must be HP, BK, BW, CF or TR'
    return(errorMsg)
  }
  

  if (toupper(trendOrCycle) == 'T' || toupper(trendOrCycle) == 'TREND'){
    as.numeric(filtered$trend)
  } else if (toupper(trendOrCycle) == 'C' || toupper(trendOrCycle) == 'CYCLE'){
    as.numeric(filtered$cycle)
  } else {
    errorMsg = '# trendOrCycle must be T (or TREND) or C (or CYCLE)'
    return(errorMsg)
  }

  #print(unemp.hp)
  #summary(unemp.hp)
  #residuals(unemp.hp)
  #fitted(unemp.hp)
  #plot(unemp.hp)

}

})