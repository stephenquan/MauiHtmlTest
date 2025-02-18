#!/bin/bash -xe

mkdir -p build-fonts

fantasticon --config myfont-sub.js
cp build-fonts/myfont-sub.ttf ../Resources/fonts/myfont-sub.ttf

fantasticon --config myfont-sup.js
cp build-fonts/myfont-sup.ttf ../Resources/fonts/myfont-sup.ttf


