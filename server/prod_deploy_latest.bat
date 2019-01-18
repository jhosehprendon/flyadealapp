@REM | This batch can be used in PROD to quickly update the webservers to the latest code on master branch, dist folder.


@echo off


PATH=%PATH%;"%ProgramFiles%"\Git\bin\;



SET APPROOT=C:\app\


@rem SET APPSOURCE=%APPROOT%src\master\

SET APPPUB=%APPROOT%publish\master\


SET TEMPFOLDER=%APPROOT%temp



echo Pulling source code from github..



git clone https://github.com/MatchByte/Flyadeal-IBE.git %TEMPFOLDER%\




@rem xcopy "%TEMPFOLDER%\*.*" "%APPSOURCE%" /H /Y /E

c:\windows\system32\inetsrv\appcmd stop apppool /apppool.name:net-core-pool
xcopy "%TEMPFOLDER%\dist\*.*" "%APPPUB%" /H /Y /E

@echo off &setlocal
cd c:\app\publish\master
set "search=</system.webServer>"
set "replace=<httpProtocol><customHeaders><remove name="X-Powered-By" /></customHeaders></httpProtocol></system.webServer>"
set "textfile=web.config"
set "newfile=temp.web.config.txt"
(for /f "delims=" %%i in (%textfile%) do (
    set "line=%%i"
    setlocal enabledelayedexpansion
    set "line=!line:%search%=%replace%!"
    echo(!line!
    endlocal
))>"%newfile%"
del %textfile%
rename %newfile%  %textfile%

c:\windows\system32\inetsrv\appcmd start apppool /apppool.name:net-core-pool



rmdir %TEMPFOLDER% /s /q



echo DONE cloning from git

pause




