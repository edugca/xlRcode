# This collection of functions provides useful functions
#
# Author: Eduardo G C Amaral
# Last update: September 10, 2022
#
# Use at your own risk


# Load R packages used by the scripts

local({
  
        #
        # List of necessary packages to be installed from compatible repository
        #
        #list.of.packages <- c("dplyr", "tidyselect")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages)

})

##################################################
# FUNCTIONS EXPOSED TO EXCEL
##################################################

RestartR <- function() {

  quit()

}

##################################################
# LOCAL FUNCTIONS
##################################################

util <- new.env()

util$cdateXLtoR <- function(datas) {

    return( as.Date(datas, origin = "1899-12-30") )

}

util$cdateRtoXL <- function(datas) {

    return( as.Date(datas) -as.Date(0, origin="1899-12-30", tz='UTC') )

}

util$installAndLoad <- function(x) {
                                    if (!require(x, character.only = TRUE))
                                    {
                                        install.packages(x, dependencies = TRUE, repos='http://cran.us.r-project.org')
                                        library(x, character.only = TRUE)
                                    }
                                }