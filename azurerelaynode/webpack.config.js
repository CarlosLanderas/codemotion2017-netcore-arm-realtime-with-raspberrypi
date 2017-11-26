const path = require('path');
const webpack = require('webpack');

module.exports = {
    entry: './app/index.js',
    output: {
        path: path.resolve(__dirname, 'public/js/'),
        publicPath: "js/",
        filename: "[name].bundle.js"
    },
    devtool: 'inline-source-map',
    module: {
        loaders: [
            {
                loader: 'babel-loader',
                test: /\.js$/,
                exclude: [/(node_modules)/],
                query: {                   
                    presets: ['es2015', 'react', 'stage-2']                   
                }
            },
            {
                test: /\.(jpeg|jpg|png|svg|gif|eot|ttf|woff|woff2)$/,
                loader: "file-loader?name=img/[name].[ext]"
            }    
        ]
    },

}
