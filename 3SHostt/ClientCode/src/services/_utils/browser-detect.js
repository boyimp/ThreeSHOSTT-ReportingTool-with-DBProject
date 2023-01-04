//@ts-check
import Bowser from 'bowser';
const browser = Bowser.getParser(window.navigator.userAgent);

export const isSafari = () => {
    console.log('Detecting browser,', Bowser.parse(window.navigator.userAgent));
    return browser.getBrowser()?.name?.toLocaleLowerCase() === 'safari';
};
