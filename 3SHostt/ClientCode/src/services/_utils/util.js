export const getRouteValue = (routeName, routePath = window.location.hash) => {
    const paths = routePath.split('/');
    return paths[paths.indexOf(routeName) + 1];
};

export const getFormatedFileSize = (sizeInByte) => {
    if (sizeInByte >= 1024 * 1024 * 1024) {
        return (sizeInByte / (1024 * 1024 * 1024)).toFixed(1) + ' GB';
    } else if (sizeInByte >= 1024 * 1024) {
        return (sizeInByte / (1024 * 1024)).toFixed(1) + ' MB';
    } else if (sizeInByte >= 1024) {
        return (sizeInByte / 1024).toFixed(1) + ' KB';
    } else {
        return sizeInByte + ' byte';
    }
};

export const isVideoFile = (fileName) => {
    var ext = getFileExtension(fileName);
    return _SupportedVideoFormat.hasOwnProperty(ext) ? 'YES' : 'NA';
};
export const getFileExtension = (fileName) => {
    if (fileName) return fileName.slice(fileName.lastIndexOf('.') + 1).toLowerCase();
    else return null;
};
export const getBrowserAdjustedTime = (d = new Date(), otherway, timeZone) => {
    var DateTime = window.luxon.DateTime;

    var timezone = 'system';

    if (
        window.UserRegionalSettings.IsLocalTimezoneSelected === false &&
        window.UserRegionalSettings.TimeZone
    ) {
        timezone = window.UserRegionalSettings.TimeZone;
    }

    var local = DateTime.fromJSDate(d, { zone: timezone });
    var adjustmentMilliSeconds = (-d.getTimezoneOffset() + -local.offset) * 60000;

    if (otherway) adjustmentMilliSeconds = -adjustmentMilliSeconds;

    return new Date(d.getTime() + adjustmentMilliSeconds);
};

export const getDateOnlyISOString = (dateValue) => {
    var dateString =
        dateValue.getFullYear() +
        '-' +
        getMonth(dateValue) +
        '-' +
        getDate(dateValue) +
        'T00:00:00.00Z';
    return dateString;
};

export const hasFieldValue = (field, fieldValue) => {
    const DEFAULT_ID = '000000000000000000000000';
    switch (field.FieldType) {
        case 'SingleLineTextField':
        case 'MultiLineTextField':
        case 'HyperlinkField':
            if (fieldValue?.length > 0) return true;
            return false;
        case 'SeparatorField':
            return true;
        case 'BooleanField':
            return fieldValue || false;
        case 'FileField':
        case 'MainFileField':
            return fieldValue?.IsFile;
        case 'CategoryField':
            return (
                fieldValue?.Id &&
                (fieldValue.Id !== DEFAULT_ID ||
                    (!!fieldValue.Label && fieldValue.Id === DEFAULT_ID))
            );
        case 'TagField':
            return fieldValue?.$values && fieldValue?.$values?.length > 0;
        case 'DateTimeField':
            return fieldValue && new Date(fieldValue).toString() !== 'Invalid Date';
        case 'LiveLinkField':
            return fieldValue?.Id !== field?.DefaultValue.Id;
        case 'UserSelectorField':
            return Array.isArray(fieldValue?.$values) && fieldValue.$values.length > 0;
        case 'TreeField':
            return fieldValue && fieldValue?.Path?.length > 0;
        case 'CalculationField':
        case 'NumberField':
            return !isNaN(fieldValue);
        default:
            return true;
    }
};

function getMonth(date) {
    var month = date.getMonth() + 1;
    return month < 10 ? '0' + month : '' + month;
}

function getDate(date) {
    var day = date.getDate();
    return day < 10 ? '0' + day : '' + day;
}

const _SupportedVideoFormat = {
    '3gp': ['video/3gpp', 'video/mp4'],
    '3gpp': ['video/3gpp'],
    '3g2': ['video/3gpp2'],
    '3gp2': ['video/3gpp2'],
    anx: ['video/ogg'],
    avi: ['video/x-msvideo'],
    f4v: ['video/mp4'],
    flv: ['video/x-flv', 'video/mp4'],
    m4v: ['video/mp4'],
    mkv: ['video/x-matroska', 'video/mp4'],
    mov: ['video/quicktime', 'video/mp4'],
    mp4: ['video/mp4'],
    mp3: ['audio/mpeg'],
    mp4v: ['video/mp4'],
    mpeg: ['video/mpeg'],
    mpg: ['video/mpeg'],
    ogg: ['video/ogg'],
    ogv: ['video/ogg'],
    webm: ['video/webm'],
    wmv: ['video/x-ms-wmv', 'video/mp4'],
    swf: ['application/x-shockwave-flash']
};
const _SupportedAudioFormat = {
    mp3: ['audio/mpeg'],
    wav: ['audio/wav'],
    m4a: ['audio/m4a'],
    ogg: ['audio/ogg'],
    wma: ['audio/wma'],
    aac: ['audio/aac'],
    aiff: ['audio/aiff'],
    flac: ['audio/flac']
};
const _SupportedImageFormat = {
    ai: '',
    bmp: '',
    jpeg: '',
    jpg: '',
    png: '',
    psd: '',
    tif: '',
    tiff: '',
    gif: '',
    eps: ''
};
const _FileIcons = {
    ai: 'ai-icon.png',
    avi: 'avi-icon.png',
    doc: 'doc-icon.png',
    docx: 'doc-icon.png',
    eps: 'eps-icon.png',
    flv: 'flv-icon.png',
    gif: 'gif-icon.png',
    ind: 'indd-icon.png',
    indd: 'indd-icon.png',
    jpg: 'jpg-icon.png',
    jpeg: 'jpg-icon.png',
    mov: 'mov-icon.png',
    mpg: 'mpg-icon.png',
    mpeg: 'mpg-icon.png',
    ogv: 'ogv-icon.png',
    pdf: 'pdf-icon.png',
    png: 'png-icon.png',
    ppt: 'ppt-icon.png',
    pptx: 'ppt-icon.png',
    psd: 'psd-icon.png',
    rar: 'rar-icon.png',
    swf: 'swf-icon.png',
    tif: 'tif-icon.png',
    tiff: 'tif-icon.png',
    wav: 'wav-icon.png',
    xls: 'xls-icon.png',
    xlsx: 'xls-icon.png',
    zip: 'zip-icon.png',
    ico: 'ico-icon.png',
    ps: 'ps-icon.png',
    svg: 'svg-icon.png',
    emf: 'emf-icon.png',
    bmp: 'bmp-icon.png',
    webp: 'webp-icon.png',
    webm: 'webm-icon.png',
    wmf: 'wmf-icon.png',
    mp4: 'mp4-icon.png',
    wmv: 'wmv-icon.png',
    mp3: 'audio-common.png',
    wav: 'audio-common.png',
    m4a: 'audio-common.png',
    aac: 'audio-common.png',
    wma: 'audio-common.png',
    flac: 'audio-common.png',
    ogg: 'audio-common.png',
    aiff: 'audio-common.png'
};
