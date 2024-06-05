dotnet sonarscanner begin /k:"ThrivoHR" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_aa5c028cc41d20bf4020fe50ea1c7dffcaccbae1"
dotnet build
dotnet sonarscanner end /d:sonar.token="sqp_aa5c028cc41d20bf4020fe50ea1c7dffcaccbae1"