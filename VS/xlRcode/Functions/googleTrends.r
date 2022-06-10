# This collection of functions provides access to Google Trends
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

        #list.of.packages <- c("curl")
        #packageurl <- "https://cran.r-project.org/src/contrib/Archive/curl/curl_4.2.tar.gz"
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(packageurl, repos=NULL, type="source")

        #list.of.packages <- c("gtrendsR")
        #packageurl <- "https://cran.r-project.org/src/contrib/Archive/gtrendsR/gtrendsR_1.4.7.tar.gz"
        #new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
        #if(length(new.packages)) install.packages(packageurl, repos=NULL, type="source")

})

##################################################
# FUNCTIONS EXPOSED TO EXCEL
##################################################

GoogleTrend <- function(palavrasChave, local = 'BR', datas = 'all', substitutoMenosQue1 = 0.5) {

  library(gtrendsR)

  palavrasChave <- as.vector(palavrasChave)
  local <- as.vector(local)
  datas <- as.vector(datas)

   # Remove empty values
  palavrasChave <- palavrasChave[complete.cases(palavrasChave)]
  local <- local[complete.cases(local)]
  datas <- datas[complete.cases(datas)]

  # Encode as URL the keywords (accents and spaces)
  palavrasChave <- URLencode(palavrasChave)

  if( length(datas) > 2 ) {

    return('# Datas podem ser texto ou vetor com data inicial e data final')

  } else if ( length(datas) == 2 ) {

    datas <- paste( format( util$cdateXLtoR( datas[[1]] ), '%Y-%m-%d'), 
                    format( util$cdateXLtoR( datas[[2]] ), '%Y-%m-%d'), 
                    sep = ' ' )

  } else if ( is.numeric(datas) ) {

    datas <- format(datas, '%y-%m-%d')

  }

  print(datas)

  queryResult <- gtrends(keyword = palavrasChave, geo = local, time = datas, onlyInterest = TRUE)

  #res <- queryResult$interest_over_time[,c('date', 'hits')]

  interestOverTime <- queryResult$interest_over_time

  interestOverTime[, 'date'] <- util$cdateRtoXL( interestOverTime[, 'date'] )

  # Replace string values "<1"
  hits <- interestOverTime[, 'hits']
  interestOverTime[hits == '<1', 'hits'] <- substitutoMenosQue1
  interestOverTime[, 'hits'] <- as.numeric( interestOverTime[, 'hits'] )

  # Decode from URL the keywords (accents and spaces)
  interestOverTime[, 'keyword'] <- URLdecode(interestOverTime[, 'keyword'])

  return(interestOverTime)

}