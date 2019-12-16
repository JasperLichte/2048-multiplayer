const fse = require('fs-extra');

(async ({rootDir, targetDir, dirsToDeploy, filesToDeploy}) => {
  try {
    dirsToDeploy.forEach(async dir => {
      await fse.copy(rootDir + dir, targetDir + dir);
    });

    filesToDeploy.forEach(async file => {
      await fse.copy(rootDir + file, targetDir + file);
    });
    console.log(
      `Files are ready to be deployed: ${await fse.realpath(targetDir)}`
    );
  } catch (e) {
    console.error(e);
  }
})({
  rootDir: './',
  targetDir: '../server/wwwroot/',
  dirsToDeploy: [
    'assets',
  ],
  filesToDeploy: [
    'index.html',
  ],
});
