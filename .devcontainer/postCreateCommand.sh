#!/bin/bash
echo "正在下載 Unity DLL 和 PDB..."
mkdir -p dlls_and_pdbs
curl -L https://github.com/wcg316/Self-directed_learning/releases/download/v0.1.0/ScriptAssemblies.zip -o ScriptAssemblies.zip
unzip ScriptAssemblies.zip -d dlls_and_pdbs
rm ScriptAssemblies.zip
echo "解壓縮完成"
