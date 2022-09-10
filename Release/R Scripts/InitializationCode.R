installAndLoad <- function(x) {
                                    if (!require(x, character.only = TRUE))
                                    {
                                        install.packages(x, dependencies = TRUE, repos='http://cran.us.r-project.org')
                                        library(x, character.only = TRUE)
                                    }
                                }

convertDates <- function(x) {lapply(x, FUN = as.Date, origin = '1899-12-30')}