import Electron, { ipcMain, ipcRenderer } from "electron";

const isMainProc = process.type === "browser";

let webContents;

const IpcProxy = {
  on: (channel, listener) => {
    if (Electron) {
      (isMainProc ? ipcMain : ipcRenderer).on(channel, listener);
    }
  },

  once: (channel, listener) => {
    if (Electron) {
      (isMainProc ? ipcMain : ipcRenderer).on(channel, listener);
    }
  },

  send: (channel, data) => {
    if (Electron) {
      (isMainProc ? webContents : ipcRenderer).send(channel, data);
    }
  },

  setWebContents: (_webContents) => {
    if (Electron) {
      webContents = _webContents;
    }
  },

  invoke: (channel, data) =>
    new Promise((resolve) => {
      IpcProxy.once(`${channel}_REPLY`, (event, data) => resolve(data));
      IpcProxy.send(channel, data);
    }),

  define: (channel, processor) => {
    IpcProxy.on(channel, async (event, data) =>
      IpcProxy.send(`${channel}_REPLY`, await processor(data))
    );
  },
};

export default IpcProxy;
