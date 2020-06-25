import { app, shell, BrowserWindow, Menu } from "electron";
import Path from "path";
import Url from "url";

import { findTvs, findEpisodes, findStream } from "../tv/api";
import packageInfo from "../../package.json";

import IpcProxy from "../ipc/proxy";
import IpcEvent from "../ipc/event";

const isDevelopment = process.env.NODE_ENV !== "production";

let mainWindow;

function createMainWindow() {
  // Create browser window.
  const window = new BrowserWindow({
    webPreferences: {
      nodeIntegration: true,
      webSecurity: false,
    },
  });

  // Initialize application menu.
  Menu.setApplicationMenu(
    Menu.buildFromTemplate([
      {
        label: "View",
        submenu: [
          {
            label: "Toggle Searchbar (Ctrl-Shift-F)",
            click: () => IpcProxy.send(IpcEvent.TOGGLE_SEARCH_BAR),
          },
        ],
      },
      {
        label: "Help",
        submenu: [
          {
            label: "Home Page",
            click: () => shell.openExternal(packageInfo.homepage),
          },
          {
            label: "Report Issue",
            click: () => shell.openExternal(packageInfo.bugs.url),
          },
          {
            label: "Check Update",
            click: () => shell.openExternal(packageInfo.homepage + "/releases"),
          },
        ],
      },
    ])
  );

  if (isDevelopment) {
    window.webContents.openDevTools();
  }

  window.loadURL(
    isDevelopment
      ? `http://localhost:${process.env.ELECTRON_WEBPACK_WDS_PORT}`
      : Url.format({
          pathname: Path.join(__dirname, "index.html"),
          protocol: "file",
          slashes: true,
        })
  );

  window.on("closed", () => (mainWindow = null));

  window.webContents.on("devtools-opened", () => {
    window.focus();
    setImmediate(() => window.focus());
  });

  // Initialize ipc-proxy.
  IpcProxy.setWebContents(window.webContents);

  // Define remote functions for ui-process.
  IpcProxy.define(IpcEvent.FIND_TVS, ({ keyword }) => {
    return findTvs(keyword);
  });

  IpcProxy.define(IpcEvent.FIND_EPISODES, ({ providerId, tvId }) => {
    return findEpisodes(providerId, tvId);
  });

  IpcProxy.define(IpcEvent.FIND_STREAM, ({ providerId, tvId, episodeId }) => {
    return findStream(providerId, tvId, episodeId);
  });

  return window;
}

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});

app.on("activate", () => {
  if (mainWindow === null) {
    mainWindow = createMainWindow();
  }
});

app.on("ready", () => {
  mainWindow = createMainWindow();
});
