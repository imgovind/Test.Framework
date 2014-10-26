PROJECT=$(basename $PWD);
CSPROJ="$PROJECT.csproj";
echo $CSPROJ;
PURPOSE=${PROJECT##*\.}
echo $PURPOSE;
ARR=(${PROJECT//./ })
echo ${ARR[3]}