$WshShell = New-Object -ComObject WScript.Shell
$desktop = [Environment]::GetFolderPath("Desktop")

$atalho = $WshShell.CreateShortcut("$desktop\FrutNatura.lnk")
$atalho.TargetPath = "C:\caminho\para\sua\FrutNatura2\iniciar_sistema.bat"
$atalho.WorkingDirectory = "C:\caminho\para\sua\FrutNatura2"
$atalho.IconLocation = "C:\caminho\para\sua\FrutNatura2\forms\Tela desktop\icone_frutnatura.ico"
$atalho.Save()

Write-Host "? Atalho criado na área de trabalho!"
