;
(function ($, window, document, undefined) {

    var pluginName = "metisMenu",
        defaults = {
            toggle: true
        };

    function Plugin(element, options) {
        this.element = element;
        this.settings = $.extend({}, defaults, options);
        this._defaults = defaults;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype = {
        init: function () {

            var $this = $(this.element),
                $toggle = this.settings.toggle;

            $this.find('li.active').has('ul').children('ul').addClass('collapse in');
            $this.find('li').not('.active').has('ul').children('ul').addClass('collapse');

            $this.find('li').has('ul').children('a').on('click', function (e) {
                e.preventDefault();
                var $a = $(this);
                $(this).parent('li').toggleClass('active').children('ul').collapse('toggle');

                if ($toggle) {
                    $(this).parent('li').siblings().removeClass('active').children('ul.in').collapse('hide');
                }

                // Auto Scroll document when click
                setTimeout(function () {
                    if ($.cookie('header') == 'header-fixed') {
                        // when sidebar fixed
                    } else {
                        if ($this.height() > $('#page-wrapper').height()) {
                            $('#wrapper').css('min-height', $this.height());
                            $('#page-wrapper').css('min-height', $this.height());
                        }
                        //if ($a.parent().hasClass('active')) {
                        //    $('html,body').animate({
                        //        scrollTop: ($a.offset().top - 100)
                        //    }, 'slow');
                        //    // Scroll to top when collapsed
                        //} else {

                        //}
                        /*$('html,body').animate({
                         scrollTop: ($a.offset().top-100)
                         }, 500); */
                    }
                }, 300);
            });
        }
    };

    $.fn[ pluginName ] = function (options) {
        return this.each(function () {
            if (!$.data(this, "plugin_" + pluginName)) {
                $.data(this, "plugin_" + pluginName, new Plugin(this, options));
            }
        });
    };

})(jQuery, window, document);
