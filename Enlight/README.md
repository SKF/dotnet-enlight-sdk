# dotnet-enlight-sdk

SDK for accessing Enlight from .NET

## services

- IoT: Internet of Things

## (Re)generate the .NET gRPC files

### Windows

set PROTOC=%UserProfile%/.nuget/packages/grpc.tools/1.13.1/tools/windows_x64/protoc.exe
set PLUGIN=%UserProfile%/.nuget/packages/grpc.tools/1.13.1/tools/windows_x64/grpc_csharp_plugin.exe

cd API
%PROTOC% -I . --csharp_out . *.proto --grpc_out . --plugin protoc-gen-grpc=%PLUGIN%

### Linux

PROTOC=~/.nuget/packages/grpc.tools/1.13.1/tools/linux_x64/protoc
PLUGIN=~/.nuget/packages/grpc.tools/1.13.1/tools/linux_x64/grpc_csharp_plugin

cd API
$PROTOC -I . --csharp_out . *.proto --grpc_out . --plugin protoc-gen-grpc=$PLUGIN
