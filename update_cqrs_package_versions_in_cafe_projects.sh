#!/bin/bash

if [ -z "$1" -o -z "$2" ]; then echo "Expected syntax: ./update_cqrs_package_versions_in_cafe_projects [old_version_number] [new_version_number]"; exit; fi

find ./src/Cafe/ -name '*csproj' -exec sed -i "s/$1/$2/g" {} \;