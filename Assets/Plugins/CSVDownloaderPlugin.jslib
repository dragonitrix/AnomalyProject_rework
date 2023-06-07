var CSVDownloaderPlugin = {
  CSVDownloader: function(str, fn) {
      var msg = Pointer_stringify(str);
      var fname = Pointer_stringify(fn);
      var contentType = 'text/csv';
      var data = new Blob([msg], {type: contentType});
      var link = document.createElement('a');
    link.download = fname;
    link.innerHTML = 'DownloadFile';
    link.setAttribute('id', 'CSVDownloaderLink');
    if(window.webkitURL != null)
    {
        link.href = window.webkitURL.createObjectURL(data);
    }
    else
    {
        link.href = window.URL.createObjectURL(data);
        link.onclick = function()
        {
            var child = document.getElementById('CSVDownloaderLink');
            child.parentNode.removeChild(child);
        };
        link.style.display = 'none';
        document.body.appendChild(link);
    }
    link.click();
  }
};
mergeInto(LibraryManager.library, CSVDownloaderPlugin);