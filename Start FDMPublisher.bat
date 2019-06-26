cd ..
cd DDS\DDS_Windows
call release.bat
ospl start file://DDSDomain.xml

cd ..\..
cd FDMPublisher\FDMPublisher\bin\Debug
call FDMPublisher.exe
