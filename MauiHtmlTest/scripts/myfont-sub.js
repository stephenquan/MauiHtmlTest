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

for (let i = 0x20; i <= 0x7e; i++)
    module.exports.codepoints[ `c${i.toString(16)}` ] = i;
