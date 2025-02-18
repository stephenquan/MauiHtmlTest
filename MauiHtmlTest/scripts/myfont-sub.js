module.exports = {
    name: 'myfont-sub',
    fontHeight: 500,
    normalize: true,
    inputDir: 'myfont-sub',
    outputDir: 'build-fonts',
    fontTypes: ['ttf'],
    assetTypes: ['css', 'json', 'html'],
    formatOptions: {
        json: {
            indent: 2
        }
    },
    codepoints: {
        'c048' : 48,
        'c049' : 49,
        'c050' : 50,
        'c051' : 51,
        'c052' : 52,
        'c053' : 53,
        'c054' : 54,
        'c055' : 55,
        'c056' : 56,
        'c057' : 57,
    },
    getIconId: ({
        basename, // `string` - Example: 'foo';
        relativeDirPath, // `string` - Example: 'sub/dir/foo.svg'
        absoluteFilePath, // `string` - Example: '/var/icons/sub/dir/foo.svg'
        relativeFilePath, // `string` - Example: 'foo.svg'
        index // `number` - Example: `0`
    }) => {
        return basename;
    }
};
