#!/bin/bash
find ./src/Cafe/ -name '*csproj' -exec sed -i "s/$1/$2/g" {} \;