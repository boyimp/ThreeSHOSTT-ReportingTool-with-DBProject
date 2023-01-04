//@ts-check

export const _SupportedVideoFormat = {
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
    mp4v: ['video/mp4'],
    mpeg: ['video/mpeg'],
    mpg: ['video/mpeg'],
    ogg: ['video/ogg'],
    ogv: ['video/ogg'],
    webm: ['video/webm'],
    wmv: ['video/x-ms-wmv', 'video/mp4'],
    swf: ['application/x-shockwave-flash']
};
export const _SupportedAudioFormat = {
    mp3: ['audio/mpeg'],
    wav: ['audio/wav'],
    m4a: ['audio/m4a'],
    ogg: ['audio/ogg'],
    wma: ['audio/wma'],
    aac: ['audio/aac'],
    aiff: ['audio/aiff'],
    flac: ['audio/flac']
};
export const _SupportedImageFormat = {
    ai: '',
    bmp: '',
    jpeg: '',
    jpg: '',
    png: '',
    psd: '',
    tif: '',
    tiff: '',
    gif: '',
    eps: '',
    svg: '',
    ico: '',
    webp: '',
    ind: '',
    indd: '',
    ps: ''
};

export const _SupportedOfficeFormat = {
    doc: '',
    docx: '',
    ppt: '',
    pptx: '',
    xls: '',
    xlsx: '',
}

export const _FileIcons = {
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

const pattern = /[/:?"<>*\\|]/;
export const IsFileNameValid = (filename) => {
    if (filename) {
        if (filename.match(pattern)) return true;
        else return false;
    }
    return null;
};

export const IsFileNameLengthValid = (filename) => {
    if (filename) {
        if (filename.length > 255) return true;
        else return false;
    }
    return null;
};

export const IsRestrictedFileType = (restrictedFileTypes, filename)=> {
    var fileExtension = GetFileExtension(filename);
    return restrictedFileTypes.indexOf(fileExtension) > -1;
}

export const GetFileTypeIconUrl = (filename) => {
    var url = 'ClientCode/Styles/Images/fileicons';
    var ext = GetFileExtension(filename);
    if (ext) {
        var fileUrl = _FileIcons[ext] ? _FileIcons[ext] : 'no-icon.png';
        return url + '/' + fileUrl;
    }
    else {
        return url + '/' + 'no-icon.png';
    }
}

export const GetFileExtension = (fileName) => {
    if (fileName) return fileName.slice(fileName.lastIndexOf('.') + 1).toLowerCase();
    else return null;
};

export const IsVideoFile = (fileName) => {
    var ext = GetFileExtension(fileName);
    return _SupportedVideoFormat.hasOwnProperty(ext);
};
export const IsAudioFile = (fileName) => {
    var ext = GetFileExtension(fileName);
    return _SupportedAudioFormat.hasOwnProperty(ext);
};

export const IsImageFile = (fileName) => {
    var ext = GetFileExtension(fileName);
    return _SupportedImageFormat.hasOwnProperty(ext);
};

export const IsOfficeFile = (fileName) => {
    var ext = GetFileExtension(fileName);
    return _SupportedOfficeFormat.hasOwnProperty(ext);
};

export const GetFileNameWithoutExtension = (fileName) => {    
    if (fileName) return fileName.slice(0, fileName.lastIndexOf('.'));
    else return null;
}
