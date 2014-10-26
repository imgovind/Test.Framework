PROJECT=$(basename $PWD);
ARR=(${PROJECT//./ })
NOW=$(date +"%m-%d-%Y %r");
NUSPEC="$PROJECT.nuspec";
COMPANY=${ARR[0]};
ASSEMBLY=${ARR[1]};
PROJECTNAME=${ARR[2]};
TECH=${ARR[3]};

echo ""
echo "----- Nuget Package Creation Process Started-----"

if [ -f $NUSPEC ];
then
	echo ""
   	echo "'$NUSPEC' exists"
   	echo ""
else
	echo "File $NUSPEC does not exists"
	nuget spec;

	sed -i 's|\$id\$|RidR|g' *.nuspec
	sed -i 's|\$title\$|RtitleR|g' *.nuspec
	sed -i 's|\$author\$|RauthorR|g' *.nuspec
	sed -i 's|\$description\$|RdescriptionR|g' *.nuspec

	sed -i "s|RidR|$PROJECT|g" *.nuspec
	sed -i "s|RtitleR|$PROJECT|g" *.nuspec
	sed -i "s|RauthorR|Govindarajan Panneerselvam|g" *.nuspec
	sed -i "s|LICENSE_URL_HERE_OR_DELETE_THIS_LINE|govind.io/opensource/$PROJECT/license.md|g" *.nuspec
	sed -i "s|PROJECT_URL_HERE_OR_DELETE_THIS_LINE|govind.io/opensource/$PROJECT/index|g" *.nuspec
	sed -i "s|ICON_URL_HERE_OR_DELETE_THIS_LINE|govind.io/opensource/$PROJECT/$PROJECT.ico|g" *.nuspec
	sed -i "s|RdescriptionR|Nuget Package for $PROJECTNAME purposes|g" *.nuspec
	sed -i "s|Summary of changes made in this release of the package.|Release Notes for $PROJECTNAME \n Initial Release -- $NOW|g" *.nuspec
	sed -i "s|Tag1 Tag2|$COMPANY $ASSEMBLY $PROJECTNAME $TECH|g" *.nuspec

	echo ""
	echo "Transformed '$NUSPEC' successfully"

	sed -i '/<\/package>/i <files>' *.nuspec
	sed -i '/<\/package>/i <file src=\"content/\**/\*.*\" target=\"content\" \/>' *.nuspec
	sed -i '/<\/package>/i <\/files>' *.nuspec

	echo ""
	echo "Added content folder to '$NUSPEC' successfully"
fi

CSPROJ="$PROJECT.csproj";

nuget pack $CSPROJ -build;

NUPKG=$(echo *.nupkg);

echo ""
echo "Created '$NUPKG' successfully"

mv *.nupkg /c/.nuget/MyPackageSource;

echo ""
echo "Moved '$NUPKG' to /c/.nuget/MyPackageSource successfully"

echo ""
echo "----- Nuget Package Creation Process Complete -----"