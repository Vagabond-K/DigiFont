let previewFont = null;

export async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

export async function updatePreviewFont(contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    if (previewFont) {
        document.fonts.delete(previewFont);
    }
    previewFont = new FontFace('PreviewFont', arrayBuffer);
    document.fonts.add(previewFont);
    await previewFont.load();
}

function updateFrame(timeStamp) {
    var date = new Date();
    var hue = (date.getSeconds() + date.getMilliseconds() / 1000) * 36;
    document.documentElement.style.setProperty('--preview-text-shadow-hue', hue.toString());
    window.requestAnimationFrame(updateFrame);
}

window.requestAnimationFrame(updateFrame);