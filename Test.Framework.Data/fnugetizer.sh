PACKAGESOURCE="/c/.nuget/MyPackageSource";
AUTHOR="Govindarajan Panneerselvam";
COMPANYURL="govind.io";
PROJECT=$(basename $PWD);
NOW=$(date +"%m-%d-%Y %r");
NUSPEC="$PROJECT.nuspec";
CONTENTDIRECTORY="Content";
APPSTARTDIRECTORY="Content\\App_Start";
MODELSDIRECTORY="Content\\Models";
CONTROLLERSDIRECTORY="Content\\Controllers";

ARR=(${PROJECT//./ })
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
	sed -i "s|RauthorR|$AUTHOR|g" *.nuspec
	if [ -n "$COMPANYURL" ]; 
	then
		sed -i "s|LICENSE_URL_HERE_OR_DELETE_THIS_LINE|$COMPANYURL/opensource/$PROJECT/license.md|g" *.nuspec
		sed -i "s|PROJECT_URL_HERE_OR_DELETE_THIS_LINE|$COMPANYURL/opensource/$PROJECT/|g" *.nuspec
		sed -i "s|ICON_URL_HERE_OR_DELETE_THIS_LINE|$COMPANYURL/opensource/$PROJECT/$PROJECT.ico|g" *.nuspec
	fi
	if [ -n "$PROJECTNAME" ]; 
	then
		sed -i "s|RdescriptionR|Nuget Package for $PROJECTNAME purposes|g" *.nuspec
		sed -i "s|Summary of changes made in this release of the package.|\n\t\tRelease Notes for $PROJECTNAME \n\t\tInitial Release -- $NOW\n\t|g" *.nuspec
		if [ -n "$TECH" ]; 
		then
			sed -i "s|Tag1 Tag2|$COMPANY $ASSEMBLY $PROJECTNAME $TECH|g" *.nuspec
		else
			sed -i "s|Tag1 Tag2|$COMPANY $ASSEMBLY $PROJECTNAME|g" *.nuspec
		fi
	else
		sed -i "s|RdescriptionR|Nuget Package for $COMPANY purposes|g" *.nuspec
		sed -i "s|Summary of changes made in this release of the package.|\n\t\tRelease Notes for $COMPANY\n\t\tInitial Release -- $NOW\n\t|g" *.nuspec
		sed -i "s|Tag1 Tag2|$COMPANY|g" *.nuspec
	fi	

	echo ""
	echo "Transformed '$NUSPEC' successfully"

	sed -i '/<\/package>/i \  <files>' *.nuspec
	if [ -d "$CONTENTDIRECTORY" ]; 
	then
		sed -i '/<\/package>/i \\t<file src=\"Content\\*.*\" target=\"Content\" \/>' *.nuspec
	fi
	if [ -d "$APPSTARTDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t<file src=\"Content\\App_Start\\*.*\" target=\"Content\\App_Start\" \/>' *.nuspec
	fi
	if [ -d "$MODELSDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t<file src=\"Content\\Models\\*.*\" target=\"Content\\Models\" \/>' *.nuspec
	fi
	if [ -d "$CONTROLLERSDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t<file src=\"Content\\Controllers\\*.*\" target=\"Content\\Controllers\" \/>' *.nuspec
	fi
	sed -i '/<\/package>/i \  <\/files>' *.nuspec

	echo ""
	echo "Added Content folder to '$NUSPEC' successfully"
fi

CSPROJ="$PROJECT.csproj";

nuget pack $CSPROJ -build;

NUPKG=$(echo *.nupkg);

echo ""
echo "Created '$NUPKG' successfully"

mv *.nupkg $PACKAGESOURCE;

echo ""
echo "Moved '$NUPKG' to /c/.nuget/MyPackageSource successfully"

echo ""
echo "----- Nuget Package Creation Process Complete -----"