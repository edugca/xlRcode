# This collection of functions provides access to Google Trends
#
# Author: Eduardo G C Amaral
# Last update: July 31, 2022
#
# Use at your own risk


DETECT <- function(text){
  
  installAndLoad('googleLanguageR')
	
  return( googleLanguageR::gl_translate(text) )
  
}

GOOGLETRANSLATE <- function(text, sourceLanguage = "", targetLanguage){
  
  installAndLoad('googleLanguageR')
	
  if (target != "")
	{
		return( gl_translate(text, source = sourceLanguage, target = targetLanguage)$translatedText )
	}
	else
	{
		return( gl_translate(text, target = targetLanguage)$translatedText )	}
  
}