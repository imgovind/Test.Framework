$project = (Get-Location | Get-Item).Name
$project += ".csproj"
nuget pack $project -build
Move-Item *.nupkg C:\.nuget\MyPackageSource -force