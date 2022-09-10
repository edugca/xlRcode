# This is a collection of specs for DESEASONALIZATION methods
#
# Author: Eduardo G C Amaral
# Last update: March 26, 2021
#
# Use at your own risk


# Create new environment
deseas_specs <- new.env()

########### These are functions that create hard-code seasonal specs
######################################################
deseas_specs$tramoseats <- function(dates, values, seriesFrequency) {

    h <- hash()

    h[["IBC-Br"]]  <- paste('SERIES { title  =  "IBC-Br"
                                    period = ',  as.character(seriesFrequency),
                                    'decimals = 2
                                    start=',  format(dates[[1]], format = "%Y.%m"),
                                    'data=(
                                      ', gsub(",", "", toString(values)) ,
                                      '
                                    )
                                    }
                                  TRANSFORM { function = log }
                                  ARIMA { model = (2 1 0)(0 1 2) }
                                  OUTLIER { span = (2015.Jan, ) }
                                  REGRESSION{
                                    variables = (lpyear LS2008.Oct LS2008.Nov TC2008.Dec)
                                    tcrate = 0.90
                                    file = "C:\\Users\\Eduardo\\Documents\\BERT2\\examples\\RegressoresDessazIBCBR.txt"
                                    format = datevalue
                                  }
                                  ESTIMATE { }
                                  X11 { seasonalma = x11default
                                        calendarsigma = all
                                        final = user
                                        save = d11}'
                                  )

    return(h)

    }

deseas_specs$X13 <- function(dates, values, seriesFrequency) {

    h <- hash()

    h[["IBC-Br"]]  <- paste('SERIES { title  =  "IBC-Br"
                                    period = ',  as.character(seriesFrequency),
                                    'decimals = 2
                                    start=',  format(dates[[1]], format = "%Y.%m"),
                                    'data=(
                                      ', gsub(",", "", toString(values)) ,
                                      '
                                    )
                                    }
                                  TRANSFORM { function = log }
                                  ARIMA { model = (2 1 0)(0 1 2) }
                                  OUTLIER { span = (2015.Jan, ) }
                                  REGRESSION{
                                    variables = (lpyear LS2008.Oct LS2008.Nov TC2008.Dec)
                                    tcrate = 0.90
                                    file = "C:\\Users\\Eduardo\\Documents\\BERT2\\examples\\RegressoresDessazIBCBR.txt"
                                    format = datevalue
                                  }
                                  ESTIMATE { }
                                  X11 { seasonalma = x11default
                                        calendarsigma = all
                                        final = user
                                        save = d11}'
                                  )

    return(h)

    }

deseas_specs$seasonal <- function(dates, values, seriesFrequency) {

    h <- hash()

    h[["IBC-Br"]]  <- paste('SERIES { title  =  "IBC-Br"
                                    period = ',  as.character(seriesFrequency),
                                    'decimals = 2
                                    start=',  format(dates[[1]], format = "%Y.%m"),
                                    'data=(
                                      ', gsub(",", "", toString(values)) ,
                                      '
                                    )
                                    }
                                  TRANSFORM { function = log }
                                  ARIMA { model = (2 1 0)(0 1 2) }
                                  OUTLIER { span = (2015.Jan, ) }
                                  REGRESSION{
                                    variables = (lpyear LS2008.Oct LS2008.Nov TC2008.Dec)
                                    tcrate = 0.90
                                    file = "C:\\Users\\Eduardo\\Documents\\BERT2\\examples\\RegressoresDessazIBCBR.txt"
                                    format = datevalue
                                  }
                                  ESTIMATE { }
                                  X11 { seasonalma = x11default
                                        calendarsigma = all
                                        final = user
                                        save = d11}'
                                  )

    return(h)

    }
