# This collection of functions provides access to DESEASONALIZATION methods
#
# Author: Eduardo G C Amaral
# Last update: April 11, 2021
#
# Use at your own risk

# Load R packages used by the scripts

local({
  
        # Define project for safe replication
        #packrat::init(paste(Sys.getenv("HOME"), "\\BERT2\\functions\\deseasonalization", sep = ""))

        #
        # Change repository so as to only get packages up to R version 3.5.X
        #  
        #r_compatible <- getOption("repos")
        #r_compatible["CRAN"] <- "https://cran.microsoft.com/snapshot/2019-04-15/" 
        #options(repos=r_compatible)

        #
        # List of necessary packages to be installed from compatible repository
        #
        #list.of.packages <- c("hash")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages)

        #
        # List of packages to be installed from specific repository
        #
        #r_specific <- getOption("repos")
        #r_specific["CRAN"] <- "https://cran.microsoft.com/snapshot/2019-04-15/" 
        #list.of.packages <- c("stringi")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages, repos = r_specific)


        #
        # List of packages to be installed from specific repository
        #
        #r_specific <- getOption("repos")
        #r_specific["CRAN"] <- "https://cran.microsoft.com/snapshot/2021-03-28/" 
        #list.of.packages <- c("RJDemetra", "xts", "TSstudio", "rlang", "hash")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages, repos = r_specific)

        #
        # List of packages to be installed from specific repository
        #
        #r_specific <- getOption("repos")
        #r_specific["CRAN"] <- "https://cran.microsoft.com/snapshot/2021-04-20/" 
        #list.of.packages <- c("seasonal", "x13binary", "forecast")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages, repos = r_specific)


        Sys.setenv(X13_PATH = "C:\\Users\\Eduardo\\OneDrive - BCB Azure\\BERT2\\lib\\x13binary\\bin")
})


##################################################
# FUNCTIONS EXPOSED TO EXCEL
##################################################


#
# Deseasonalize a time series (returns a vector)
#
Deseasonalize <- function(dates = FALSE, values = FALSE, result = 'SA', method = 'X13', specName = FALSE, specFile = FALSE, specText = FALSE){

  library(RJDemetra)
  library(xts)
  library(TSstudio)
  library(seasonal)
  library(hash)
  library(forecast)

  # Clear error message
  errorMsg = ''

  # Read calendar
  #cal_path <- '/Users/Eduardo/Documents/BERT2/examples/RegressoresDessazIBCBR.txt'
  #calendar <- read.table(cal_path, stringsAsFactors = FALSE)
  #tsCal <- ts(data = calendar[,3:ncol(calendar)], start = c(calendar[1,1], calendar[1,2]), frequency = 12)

  # Check dimension of dates
  if( is.null(nrow(dates)) ){
    errorMsg = '# Dates length must be larger than 1'
    return(errorMsg)
  }

  # Check dimension of values
  if( is.null(nrow(values)) ){
    errorMsg = '# Values length must be larger than 1'
    return(errorMsg)
  }

  # Check if dates and values are numeric
  if ( !is.numeric(dates) ){
    errorMsg = '# Dates vector contains non-numeric values'
    return(errorMsg)
  }
  if ( !is.numeric(values) ){
    errorMsg = '# Values vector contains non-numeric values'
    return(errorMsg)
  }

  # Check if dates are sorted in ascending order
  if ( is.unsorted(dates) ){
    errorMsg = '# Dates vector must be in ascending order'
    return(errorMsg)
  }

  # Check matrix dimensions of dates
  if (ncol(dates) == 1 && nrow(dates) > 1) {
    # 1 column to N rows
    rowOriented <- TRUE
    dates <- as.Date(dates, origin = '1899-12-30') # Convert dates from Excel format to R
  } else if (ncol(dates) > 1 && nrow(dates)== 1) {
    # 1 row to N cols
    rowOriented <- FALSE
    dates <- as.Date(t(dates), origin = '1899-12-30') # Convert dates from Excel format to R
  } else {
    errorMsg = '# Dates must be 1 row or 1 column'
    return(errorMsg)
  }

  # Check matrix dimensions of values
  if (ncol(values) == 1 && nrow(values) > 1) {
    # 1 column to N rows
    nSeries <- 1
  } else if (ncol(values) > 1 && nrow(values)== 1) {
    # 1 row to N cols
    nSeries <- 1
    values <- t(values)
  } else if (nrow(values) == length(dates)) {
    # N columns to Dates rows
    nSeries <- ncol(values)
  } else {
    errorMsg = '# Values must be 1 row, 1 column, or N columns with the same number of rows as dates'
    return(errorMsg)
  }

  # Check number of elements
  nDates <- nrow(dates)
  nValues <- nrow(values)
  if (nDates != nValues){
    errorMsg = '# Each series must have one value for each date'
    return(errorMsg)
  }

  # Keep indexes
  firstDateIdx <- 1
  lastDateIdx <- nDates

  # Check missing dates or values
  if ( anyNA(dates) ) {
    errorMsg = '# There are missing dates'
    return(errorMsg)
  }
  if ( anyNA(values) ) {
    # Check missing values in-between values
    #if ( !all(abs(diff( which(is.na(values)) )) <= 1) ) {
    #errorMsg = '# There are missing values'
    #return(errorMsg)
    #}

    # Check missing values in the beginning or end
    if ( all(abs(diff( which(is.na(values)) )) <= 1) ) {
      
      # There are missing values in the beginning and/or end
      firstDateIdx <- which( !is.na(values) )[1]
      lastDateIdx <- tail( which( !is.na(values) ), 1)
      dates <- dates[firstDateIdx:lastDateIdx, 1]
      values <- as.matrix(values[firstDateIdx:lastDateIdx, ])
      
    } else {
        # There are missing values irregularly distributed
        errorMsg = '# There are missing values'
        return(errorMsg)
    }
 
  }

  # Check dates are ascending, regularly spaced, and have aceeptable frequency (monthly, quarterly, biannual)
  datesCheck <- seq(dates[1], length.out = length(dates), by = "month")
  if (any(dates != datesCheck)){
    # Not monthly
    datesCheck <- seq(dates[1], length.out = length(dates), by = "quarter")
    if (any(dates != datesCheck)){
      # Not quarterly
      datesCheck <- seq(dates[1], length.out = length(dates), by = "2 quarter")
      if (any(dates != datesCheck)){
        # Not biannual
        if (method != 'FORECAST'){
          errorMsg = '# Dates must be regularly spaced either monthly, quarterly or biannual'
          return(errorMsg)
        } else {
          # Other frequency is only acceptable with FORECAST method
          seriesFrequency <- NULL
        }
      } else {
          seriesFrequency <- 2
          if (nDates < 6) {  
            errorMsg = '# SA needs at least 3 years of data'
            return(errorMsg)
          }
      }
    } else {
      seriesFrequency <- 4
      if (nDates < 12) {  
        errorMsg = '# SA needs at least 3 years of data'
        return(errorMsg)
      }
    }
  } else {
    seriesFrequency <- 12
    if (nDates < 36) {  
        errorMsg = '# SA needs at least 3 years of data'
        return(errorMsg)
      }
  }

  # Check whether method is adequate
  if (toupper(method) != 'X13'
        && toupper(method) != 'TRAMO-SEATS' 
        && toupper(method) != 'SEASONAL'
        && toupper(method) != 'FORECAST') {
    errorMsg = '# Method must be X13, TRAMO-SEATS, SEASONAL or FORECAST'
    return(errorMsg)
  }

  # Load specs
  if (toupper(method) == 'X13'){
    specsList <- deseas_specs$X13(dates, values, seriesFrequency)
  } else if (toupper(method) == 'TRAMO-SEATS'){
    specsList <- deseas_specs$tramoseats(dates, values, seriesFrequency)
  } else if (toupper(method) == 'SEASONAL'){
    specsList <- deseas_specs$seasonal(dates, values, seriesFrequency)
  }

  # Check whether spec was provided and is adequate
  if (specName == FALSE) {
    
    if (specFile != FALSE) {

      specName <- specFile

    } else if (specText != FALSE) {

      # Spec was passed as text
    specName <- 'TEXT'

    } else {

      if (toupper(method) == 'X13'){
        specName <- 'X11'
      } else if (toupper(method) == 'TRAMO-SEATS'){
        specName <- 'RSAfull'
      } else if (toupper(method) == 'SEASONAL'){
        specName <- 'Default'
      }

    }
    

  } else {

    if (toupper(method) == 'X13'){

      if ( !any(c("RSA5c", "RSA0", "RSA1", "RSA2c", "RSA3", "RSA4c", "X11") == specName ) ) {
        errorMsg <- '# Spec must be "RSA5c", "RSA0", "RSA1", "RSA2c", "RSA3", "RSA4c" or "X11"'
        return(errorMsg)
      }

    } else if (toupper(method) == 'TRAMO-SEATS'){

      if ( !any(c("RSAfull", "RSA0", "RSA1", "RSA2", "RSA3", "RSA4", "RSA5") == specName ) ) {
        errorMsg <- '# Spec must be "RSAfull", "RSA0", "RSA1", "RSA2", "RSA3", "RSA4" or "RSA5"'
        return(errorMsg)
      }

    } else if (toupper(method) == 'SEASONAL'){

      if ( !any(hash::keys(specsList) == specName ) ) {
        errorMsg <- paste('# Spec must be ', paste(hash::keys(specsList), sep = ",") , sep = "")
        return(errorMsg)
      }

    }   

  }

  # Build time series
  mySeries <- NULL
  for (iSeries in 1:nSeries){
      auxSeries <- xts_to_ts(xts(values[,iSeries], order.by = dates, dateFormat='Date'), frequency = seriesFrequency, start = dates[1])
      mySeries <- cbind(mySeries, auxSeries)
  }

  # Build spec
  if (toupper(method) == 'X13') {

      mySpec_X13 <- x13_spec(spec = specName)
      #mySpec_X13 <- x13_spec(spec = specName,
      #                transform.function = 'Log',
      #                arima.p = 2,
      #                arima.d = 1,
      #                arima.q = 0,
      #                arima.bp = 0,
      #                arima.bd = 1,
      #                arima.bq = 2,
      #                tradingdays.option = 'UserDefined',
      #                outlier.tcrate = 0.90,
      #                outlier.from = '2015.Jan',
      #                usrdef.outliersType = 'LS',
      #                usrdef.outliersDate = c('lpyear', '2008-10-01', '2008-11-01', '2008-12-01'),
      #                usrdef.varType = 'Calendar',
      #                usrdef.var = tsCal,
      #                usrdef.varEnabled = TRUE,
      #                )

  } else if (toupper(method) == 'TRAMO-SEATS') {

      mySpec_TRAMO <- tramoseats_spec(spec = specName)
      #mySpec_TRAMO <- tramoseats_spec(spec = specName,
      #                  transform.function = 'Log',
      #                  arima.p = 2,
      #                  arima.d = 1,
      #                  arima.q = 0,
      #                  arima.bp = 0,
      #                  arima.bd = 1,
      #                  arima.bq = 2
      #                  )

  } else if (toupper(method) == 'SEASONAL') {

    if (specFile != FALSE ) {
      mySpec_SEAS <- seasonal::import.spc( file = specFile )
      mySpec_SEAS$seas$x <- mySeries

    } else if (specText != FALSE ) {

      mySpec_SEAS <- seasonal::import.spc( text = specText )
      mySpec_SEAS$seas$x <- mySeries
      #mySpec_SEAS <- mySeasonal$import.spc( text = specText )
      #mySpec_SEAS$x <- mySeries

    } else if (specName != 'Default') {
      mySpec_SEAS <- seasonal::import.spc( text = specsList[[specName]] )
    }

  } 

  # Deseasonalize
  model = NULL
  if (toupper(method) == 'X13') {
    # X-13ARIMA method
    out <- try( model <- x13(mySeries, spec = mySpec_X13), silent = FALSE)
  } else if (toupper(method) == 'TRAMO-SEATS') {
    # TRAMO-SEATS method
    out <- try( model <- tramoseats(mySeries, spec = mySpec_TRAMO), silent = FALSE)
  } else if (toupper(method) == 'SEASONAL') {
    # SEASONAL package method
    # model <- seas(mySeries)

    if (specName == 'Default') {
      out <- try( model <- seas(mySeries), silent = FALSE)
    } else {
      out <- try( model <- lapply(mySpec_SEAS, eval, envir = globalenv()), silent = FALSE)
    }
    
  } else if (toupper(method) == 'FORECAST') {
    # FORECAST method
    out <- try( model <- mstl(msts(mySeries, seasonal.periods=c(5, 5*2, 365.25*5/7/12, 365.25*5/7))), silent = FALSE)
  }

  if ( is.null(model) ) { 
      return(out)
  }

  # Define return
  resMdl = NULL
  if (toupper(method) == 'X13' || toupper(method) == 'TRAMO-SEATS'){
    resMdl.SA <- model$final$series[,'sa']
  } else if (toupper(method) == 'SEASONAL') {
     if (specName == 'Default') {
        resMdl.SA <- final(model)
        resMdl.RES <- irregular(model)
        resMdl.TREND <- trend(model)
    } else {
          model <- model$seas
          
          resMdl.SA <- final(model)
          resMdl.RES <- irregular(model)
          resMdl.TREND <- trend(model)

          colnames(resMdl.SA) <- NULL
          colnames(resMdl.RES) <- NULL
          colnames(resMdl.TREND) <- NULL
        }
  } else if (toupper(method) == 'FORECAST') {

    resMdl.SA <- seasadj(model)

  }


  # Build return
  retArr <- strsplit(result, ',')
  nRet <- length(retArr)
  res <- NULL
  for (retItem in retArr){
    if (toupper(retItem) == 'SA'){
      res <- cbind(res, resMdl.SA)
    } else if (toupper(retItem) == 'RES'){
      res <- cbind(res, resMdl.RES)
    } else if (toupper(retItem) == 'TREND'){
      res <- cbind(res, resMdl.TREND)
    }
  }
  
  # Resize return
  if (firstDateIdx != 1){
    res[-(firstDateIdx-1):-1, ] <- NA
  }
  if (lastDateIdx != nValues){
    res[(lastDateIdx+1):nValues, ] <- NA
  }

  # Return value
  res

}