const path = require("path");
const HTMLWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyPlugin = require("copy-webpack-plugin");
const TerserWebpackPlugin = require("terser-webpack-plugin");
const ESLintPlugin = require("eslint-webpack-plugin");

const isProd = process.env.NODE_ENV === "production";
const isDev = !isProd;
const fileName = (ext) => (isDev ? `[name].${ext}` : `[name].[hash].${ext}`);

const optimization = () => {
  const config = {
    splitChunks: {
      chunks: "all",
    },
  };

  if (isProd) {
    config.minimizer = [new TerserWebpackPlugin()];
  }
  return config;
};

const babelOptions = (preset, plugins) => {
  const opt = {
    presets: [
      "@babel/preset-env",
      [
        "@babel/preset-typescript",
        {
          isTSX: true,
          allExtensions: true,
        },
      ],
    ],
    plugins: [
      "@babel/plugin-proposal-class-properties",
      [
        "module-resolver",
        {
          extensions: [".js", ".json", ".tsx", ".ts", ".svg"],
          root: ["./src"],
          alias: {
            "@src": "./src",
          },
        },
      ],
    ],
  };
  if (plugins) {
    opt.plugins.push(plugins);
  }
  if (preset) {
    opt.presets.push(preset);
  }
  return opt;
};

module.exports = {
  context: path.resolve(__dirname, "src"),

  mode: "development",
  entry: ["@babel/polyfill", "./index.tsx"],
  output: {
    filename: fileName("js"),
    path: path.resolve(__dirname, "dist"),
    publicPath: "/",
  },

  resolve: {
    extensions: [".js", ".json", ".tsx", ".ts", ".svg"],
    alias: {
      "@src": path.resolve(__dirname, "src"),
    },
  },

  optimization: optimization(),

  devtool: isDev ? "inline-source-map" : "eval",

  devServer: {
    port: 3000,
    hot: isDev,
    historyApiFallback: true,
  },

  plugins: [
    new HTMLWebpackPlugin({
      template: "index.html",
      collapseWhitespace: isProd,
      removeComments: isProd,
    }),
    new CleanWebpackPlugin(),
    new MiniCssExtractPlugin({
      filename: fileName("css"),
    }),
    new ESLintPlugin(),
    new CopyPlugin({
      patterns: [
        {
          from: path.resolve(__dirname, "src/assets/images"),
          to: path.resolve(__dirname, "dist/images"),
        },
      ],
    }),
  ],
  module: {
    rules: [
      {
        test: /\.s[ac]ss$/i,
        use: [
          {
            loader: MiniCssExtractPlugin.loader,
          },
          "css-loader",
          "sass-loader",
        ],
      },
      {
        test: /\.css$/i,
        use: [
          {
            loader: MiniCssExtractPlugin.loader,
          },
          "css-loader",
        ],
      },
      {
        test: /\.(ttf|woff|eot)$/,
        type: "asset/resource",
      },
      {
        test: /\.m?ts$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "babel-loader",
            options: babelOptions(),
          },
        ],
      },
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "babel-loader",
            options: babelOptions(),
          },
        ],
      },
      {
        test: /\.jsx$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "babel-loader",
            options: babelOptions(),
          },
        ],
      },
      {
        test: /\.tsx$/,
        exclude: /node_modules/,
        use: [
          {
            loader: "babel-loader",
            options: babelOptions(
              ["@babel/preset-react", { runtime: "automatic" }],
              "react-hot-loader/babel"
            ),
          },
        ],
      },
      {
        test: /\.(png|jpe?g|gif|svg)$/i,
        type: "asset/resource",
      },
    ],
  },
};
