﻿var Controller = new function () {
    var that = this,  options, formModel = {}, oldTitle = document.title,  windowModel;
    function cancel() {
        windowModel.Hide(function () {
        });
        document.title = oldTitle;
    };
    function show(model) {
        windowModel.Show();
        oldTitle = document.title;
        document.title = formModel.Title = 'Add New Files';
        that.Image.OnOpen();

    };
    function createWindow(template) {
        windowModel = Global.Window.Bind(template);
        Global.Form.Bind(formModel, windowModel.View);
        windowModel.View.find('.btn_cancel').click(cancel);
        show();
        that.Image.Bind();
    };
    this.Show = function (opts) {
        options = opts;
        if (windowModel) {
            show();
        } else {
            Global.LoadTemplate('/Content/Templates/AddFiles.html', function (response) {
                createWindow(response);
            }, function (response) {
            });
        }
    };
    (function () {
        var container;
        function onDelete(model) {
            console.log("[model]", model);
            if (model.IsCompleted) {
                Global.Controller.Call({
                    url: IqraConfig.Url.Js.WarningController,
                    functionName: 'Show',
                    options: {
                        name: 'Delete',
                        msg: 'Do you want to delete?',
                        save: '/GalleryArea/Gallery/RemoveFile',
                        data: { Id: model.Id },
                        onsavesuccess: function () {
                            model.elm.remove();
                            options.onDelete();
                        }
                    }
                });
            } else {
                model.request.Cancel();
                model.elm.remove();
            }
            model.IsCompleted = true;
        };
        function save(model) {
            model.request = Global.Uploader.upload({
                data: model.Data,
                url: options.Save,
                onProgress: function (data) {
                    model.Layer.css({ width: parseInt((data.loaded / data.total) * 100) + 'px' });
                },
                onComplete: function (response) {
                    model.IsCompleted = true;
                    if (!response.IsError) {
                        model.Layer.remove();
                        options.onAdd();
                        model.Id = response.Data;
                        console.log("[model.Id]", model.Id);
                        return;
                    }
                    model.IsError = true;
                    model.Layer.css({ width: '100px' }).html('<div style="color: red; font-weight: bold; margin: 0px auto; padding: 38px 0px 38px 15px;">Error </div>');
                },
                onError: function () {
                    model.IsError = true;
                    model.IsCompleted = true;
                    model.Layer.css({ width: '100px' }).html('<div style="color: red; font-weight: bold; margin: 0px auto; padding: 38px 0px 38px 15px;">Error </div>');
                }
            });
        };
        function createView(src, file) {
            var layer = $('<div style="position: absolute; z-index: 1; background-color: rgba(20, 20, 20, 0.7); height: 100px; top: 0px; right: -10px; width: 100px;"></div>');
            var btn = $('<div class="btn_delete" style="position: absolute; right: -10px; top: -3px;z-index: 2;"><span class="icon_container" style="border-radius: 50%; padding: 0px 1px; border: 2px solid rgb(255, 255, 255); cursor: pointer; font-size: 0.8em;"> <span class="glyphicon glyphicon-remove"></span></span></div>');
            var img = $('<img style="max-width:100px; max-height:100px;" />'), elm = $('<div class="col-md-3 image_item"></div>').append(btn).append(img).append(layer);
            img.attr('src', src);
            var model = {
                Id: Global.Guid(),
                elm: elm,
                Layer: layer,
                Data: {
                    Img: { IsFile: true, File: file },
                    Id: options.Id,
                    ActivityId: window.ActivityId
                }
            };
            Global.Click(btn, onDelete, model);
            container.append(elm);
            save(model);
        };
        function readURL(reader, input, index) {
            index = index || 0;
            var reader = reader || new FileReader();
            if (input.files && input.files.length - 1 >= index) {
                var file = input.files[index];
                reader.onload = function (e) {
                    createView(e.target.result, file);
                    index++;
                    readURL(reader, input, index);
                }
                reader.readAsDataURL(file);
            }
        }
        function onChange() {
            var input = this, reader;
            readURL(reader, input);
        };
        function close() {
            windowModel && windowModel.Hide();
        };

        this.Status = function (txt) {
            if (txt == 'End') {
                windowModel.View.find('.status_container').empty();
                //windowModel.View.find('#progress_ba_container').hide();
                windowModel.View.find('#progress_ba_container #myBar').css({ width: 0 });
                close();
                callerOptions.Success();
            } else {
                windowModel.View.find('.status_container').prepend('<div class="col-md-12">' + txt + '</div>');
            }
        };
        this.Bind = function () {
            windowModel.View.find('#btn_image').change(onChange)[0];
            container = windowModel.View.find('#image_container');
        };
        this.OnOpen = function (model) {
            
        };
        this.IsValid = function () {

            return true;
        };
    }).call(that.Image = {});
}