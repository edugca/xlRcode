# This collection of functions provides useful functions
#
# Author: Eduardo G C Amaral
# Last update: March 26, 2021
#
# Use at your own risk


# Load R packages used by the scripts

local({
  
        # Define project for safe replication
        #packrat::init("~/filters")

        #
        # Change repository so as to only get packages up to R version 3.5.X
        #  
        #r_compatible <- getOption("repos")
        #r_compatible["CRAN"] <- "https://cran.microsoft.com/snapshot/2019-04-15/" 
        #options(repos=r_compatible)

        #
        # List of necessary packages to be installed from compatible repository
        #
        #list.of.packages <- c("dplyr", "tidyselect")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages)


        #
        # List of packages to be installed from specific repository
        #
        #r_specific <- getOption("repos")
        #r_specific["CRAN"] <- "https://cran.microsoft.com/snapshot/2021-03-28/" 
        #list.of.packages <- c("Rtools")
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(new.packages, repos = r_specific)

        #
        # List of packages to be installed from GITHUB
        #
        #if (!require("devtools")) install.packages("devtools")
        #devtools::install_github("PMassicotte/gtrendsR")


        #
        # List of specific versions of packages to be installed from specific links
        #
        #list.of.packages <- c("pillar")
        #packageurl <- "https://cran.r-project.org/src/contrib/Archive/pillar/pillar_1.4.3.tar.gz"
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(packageurl, repos=NULL, type="source")

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