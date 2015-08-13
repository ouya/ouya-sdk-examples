cordova.define('cordova/plugin_list', function(require, exports, module) {
module.exports = [
    {
        "file": "plugins/cordova-plugin-ouya-sdk/ouya-sdk.js",
        "id": "cordova-plugin-ouya-sdk.OuyaSDK",
        "runs": true
    },
    {
        "file": "plugins/cordova-plugin-whitelist/whitelist.js",
        "id": "cordova-plugin-whitelist.whitelist",
        "runs": true
    }
];
module.exports.metadata = 
// TOP OF METADATA
{
    "cordova-plugin-ouya-sdk": "1.0.0",
    "cordova-plugin-whitelist": "1.0.0"
}
// BOTTOM OF METADATA
});