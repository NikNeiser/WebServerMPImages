var app = (function () {
  var config = {};

  var ui = {
    $document: $(document),
    $cart: $("#cart"),
    $blank: $("#blank"),
  };

  // Привязка событий
  function _bindHandlers() {
    ui.$document.on("click", ".cover", function () {
      var mainCont = $(this).parents(".mainCont")[0];

      $(".mainCont.opened").each(function () {
        OpenClose(this);
      });

      OpenClose(mainCont);
    });

    ui.$document.on("click", ".close", function () {
      var mainCont = $(this).parents(".mainCont")[0];
      OpenClose(mainCont);
    });

    ui.$document.on("click", ".image", function () {
      if ($(this).hasClass("selected")) {
        var $cartItem = $("input[value='" + $(this).attr("id") + "']").closest(
          ".cartImage"
        );
        $cartItem.remove();

        DeselectImage($(this));
      } else {
        var $cartItem = ui.$blank.clone(true);
        $cartItem.removeAttr("id");
        $cartItem
          .children("img")
          .attr("src", $(this).children("img").attr("src"));
        $cartItem.children("input").attr("value", $(this).attr("id"));
        $cartItem.appendTo(ui.$cart);

        SelectImage($(this));
      }
    });

    ui.$document.on("click", ".deleteBtn", function () {
      var $cartItem = $(this).closest(".cartImage");
      $cartItem.remove();
      var $imageBtn = $("#" + $cartItem.children("input").attr("value"));
      DeselectImage($imageBtn);
    });

      ui.$document.on("change", "#preset-select", function () {
          
          if ($(this).val() != "") {
              var preset = presets.find(obj => { return obj.PresetName === $(this).val() });
              $("input[name='Width']").val(preset.Width);
              $("input[name='Height']").val(preset.Height);
              $("input[name='TransparentBG']").prop('checked', preset.TransparentBG).change();
              $("input[name='BGColor']").val(preset.BGColor);
              $("input[name='NameByBarcode']").prop('checked', preset.NameByBarcode);
              $("select[name='Extension']").val(preset.Extension);
          }
      });

      ui.$document.on("change", ".preset-value", function () {
          $("#preset-select").val("");
      });

      ui.$document.on("change", "input[name='TransparentBG']", function () {

          if ($(this).is(":checked")) {
              $(".BGColor").hide(500);
          } else {
              $(".BGColor").show(500);
          }
      });

      $(ui.$cart).on('DOMSubtreeModified', function () {
          var cartIsEmpty = ui.$cart.find(".cartImage").length < 2;
          $("#loadBtn").prop('disabled', cartIsEmpty);          
      });

  }

  function SelectImage($btnImage) {
    $btnImage.closest(".mainCont").addClass("selected");
    $btnImage.addClass("selected");
  }

  function DeselectImage($btnImage) {
    $btnImage.removeClass("selected");
    $imageCont = $btnImage.closest(".mainCont");
    if ($imageCont.find(".selected").length < 1) {
      $imageCont.removeClass("selected");
    }
  }

  function OpenClose(mainCont) {
    var imageCont = $(mainCont).children(".imageCont")[0];
    $(mainCont).toggleClass("closed col col-6 col-md-2 col-sm-3 opened");
    $(imageCont).toggleClass("row");
  }

    function AddPresets() {
        $.each(presets, function (i, item) {
            $('#preset-select').append($('<option>', {
                value: item.PresetName,
                text: item.PresetName
            }));
        });
    }

  // Инициализация приложения
  function init() {
    // ...
      AddPresets();
      _bindHandlers();

  }

  // Возвращаем наружу
  return {
    init: init,
  };
})();

// Запуск приложения
$(document).ready(app.init);
