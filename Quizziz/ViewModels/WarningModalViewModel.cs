using Microsoft.AspNetCore.Html;

namespace Quizziz.ViewModels
{
    public class WarningModalViewModel
    {
        public string ModalId { get; set; } = "warningModal";
        public string Title { get; set; } = "Warning";
        public IHtmlContent Content { get; set; }
        public string HeaderClass { get; set; } = "bg-warning text-dark";
        public string PrimaryButtonText { get; set; } = "";
        public string PrimaryButtonClass { get; set; } = "btn-primary";
        public string PrimaryButtonAction { get; set; } = "";
        public string SecondaryButtonText { get; set; } = "Close";
        public string SecondaryButtonClass { get; set; } = "btn-secondary";
    }
}