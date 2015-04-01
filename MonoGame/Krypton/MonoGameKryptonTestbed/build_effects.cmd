ECHO Add 2MGFX..exe to your path
PATH=C:\Users\tgraupmann\Documents\TAGENIGMA\Ouya\Public\MonoGameSickHead\Tools\2MGFX\bin\Windows\AnyCPU\Debug;%PATH%
cd effects
2MGFX.exe KryptonEffect.fx KryptonEffect.mgfx /Profile:OpenGL 
ECHO ON
COPY /Y KryptonEffect.mgfx ..\Assets\
PAUSE