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
DATADIRECTORY="Content\\Data";
COREDATADIRECTORY="Content\\Data\\Core"
EFDATADIRECTORY="Content\\Data\\EF";
PETAPOCODATADIRECTORY="Content\\Data\\PetaPoco";
SUBSONICDATADIRECTORY="Content\\Data\\SubSonic";
APPREADMEDIRECTORY="Content\\App_Readme";

ARR=(${PROJECT//./ })
COMPANY=${ARR[0]};
ASSEMBLY=${ARR[1]};
PROJECTNAME=${ARR[2]};
TECH=${ARR[3]};

if [ -n $1 ] && [ $1 = "--overwrite" ]
then
	echo "";
	echo "Overwriting nuspec";
	rm *.nuspec;
fi

echo "";
echo "----- Nuget Package Creation Process Started-----";

if [ -f $NUSPEC ];
then
	echo ""
   	echo "'$NUSPEC' exists"
   	echo ""
else
	echo "File $NUSPEC does not exists"
	nuget spec;

	sed -i 's|\$title\$|RtitleR|g' *.nuspec
	sed -i "s|RtitleR|$PROJECT|g" *.nuspec
	if [ -n "$COMPANYURL" ]; 
	then
		sed -i "s|LICENSE_URL_HERE_OR_DELETE_THIS_LINE|$COMPANYURL/opensource/$PROJECT/license.md|g" *.nuspec
		sed -i "s|PROJECT_URL_HERE_OR_DELETE_THIS_LINE|$COMPANYURL/opensource/$PROJECT/|g" *.nuspec
		sed -i "s|ICON_URL_HERE_OR_DELETE_THIS_LINE|$COMPANYURL/opensource/$PROJECT/$PROJECT.ico|g" *.nuspec
	fi
	if [ -n "$PROJECTNAME" ]; 
	then
		sed -i "s|Summary of changes made in this release of the package.|\n\t\t\tRelease Notes for $PROJECTNAME \n\t\t\tInitial Release -- $NOW\n\t\t|g" *.nuspec
		if [ -n "$TECH" ]; 
		then
			sed -i "s|Tag1 Tag2|$COMPANY $ASSEMBLY $PROJECTNAME $TECH|g" *.nuspec
		else
			sed -i "s|Tag1 Tag2|$COMPANY $ASSEMBLY $PROJECTNAME|g" *.nuspec
		fi
	else
		sed -i "s|Summary of changes made in this release of the package.|\n\t\t\tRelease Notes for $COMPANY\n\t\t\tInitial Release -- $NOW\n\t\t|g" *.nuspec
		sed -i "s|Tag1 Tag2|$COMPANY|g" *.nuspec
	fi	

	echo ""
	echo "Transformed '$NUSPEC' successfully"

	if [ -n "$TECH" ] && [ "$TECH" = "Subsonic" ]
	then
		sed -i '/<\/metadata>/i \\t  <frameworkAssemblies>' *.nuspec
		sed -i '/<\/metadata>/i \\t\t\t<frameworkAssembly assemblyName=\"System.Transactions\" \/>' *.nuspec
		sed -i '/<\/metadata>/i \\t  <\/frameworkAssemblies>' *.nuspec
	
		echo ""
		echo "Added Framework GAC assemblies successfully"
	fi

	sed -i '/<\/package>/i \  <files>' *.nuspec
	if [ -d "$CONTENTDIRECTORY" ]; 
	then
		sed -i '/<\/package>/i \\t\t<file src=\"Content\\*.*\" target=\"Content\" \/>' *.nuspec
	fi
	if [ -d "$APPSTARTDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\App_Start\\*.*\" target=\"Content\\App_Start\" \/>' *.nuspec
	fi
	if [ -d "$MODELSDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Models\\*.*\" target=\"Content\\Models\" \/>' *.nuspec
	fi
	if [ -d "$CONTROLLERSDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Controllers\\*.*\" target=\"Content\\Controllers\" \/>' *.nuspec
	fi
	if [ -d "$DATADIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Data\\*.*\" target=\"Content\\Data\" \/>' *.nuspec
	fi
	if [ -d "$COREDATADIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Data\\Core\\*.*\" target=\"Content\\Data\\Core\" \/>' *.nuspec
	fi
	if [ -d "$EFDATADIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Data\\EF\\*.*\" target=\"Content\\Data\\EF\" \/>' *.nuspec
	fi
	if [ -d "$PETAPOCODATADIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Data\\PetaPoco\\*.*\" target=\"Content\\Data\\PetaPoco\" \/>' *.nuspec
	fi
	if [ -d "$SUBSONICDATADIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\Data\\SubSonic\\*.*\" target=\"Content\\Data\\SubSonic\" \/>' *.nuspec
	fi
	if [ -d "$APPREADMEDIRECTORY" ]; 
	then
	sed -i '/<\/package>/i \\t\t<file src=\"Content\\App_Readme\\*.*\" target=\"Content\\App_Readme\" \/>' *.nuspec
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