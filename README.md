# FileRenamer
File Renamer Library
Features :</br>
  1.Rename Files With Customize Pattern.</br>
  2.Filter statements in the filenames.</br>
  3....</br>
  
##Statements Rules :
     -All the characters that are legal to use in a filename are useable
     -RANDOM : use a random string , Example:
     -RANDOM10 => a string of characters with lenght between 1 to 10
     -INDEX : use a number to set index , Example:
     -INDEX => add 1,2,3... to files
     -FILENAME : use the default name as a substring for new filename
     -EXTENSION => use default file extension
     -DATE : use current Date in the filename
     -TIME : use current Time in the filename
     
###**NOTE: Statements must be used UPPERCASE.**

Tried to use Dependency Injection.</br>
There is two Interface IPattern , IFilter</br>
with implementing them and use them in the Renamer constructor we can have custom Pattern and Filter,</br>
even though there is two default class which implement default rules for Renamer.</br>

###Some Example With Statements :
     -FILENAME	=>		Using Statements 		        => 		NewFilename
     -11.txt    =>		LogDATE EXTENSION		        => 		Log07162016 .txt
     -12.srt	=>		RANDOM5 INDEX EXTENSION	        =>		sEK3j 1 .txt
     -AMIR.exe	=>		DATE-TIME FILENAME		        =>		07162016-08-59-33 AMIR.exe

###**NOTE : `INDEX` Will incerase++ and use in the list of files.**</br>
###**NOTE : `RANDOM` Get a Number which stick to `RANDOM` statement and must get a whitespace after parameter.**</br>

###For More Details and Use this Library in a Windows application [FileRenamerAdvanced](https://github.com/amirhosseinghorbani/FileRenamerAdvanced) 
