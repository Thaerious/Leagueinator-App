using DragDropEffects = System.Windows.Forms.DragDropEffects;
using DragEventArgs = System.Windows.Forms.DragEventArgs;
using DragEventHandler = System.Windows.Forms.DragEventHandler;

namespace Leagueinator.Components {

    class DataPacket<T> {
        public T? data;
        public T? response;
    }

    public class ControlDragHandlers<T> {
        private readonly Control control;
        private readonly Func<T?> getData;
        private readonly Func<T?, T?> sendData;
        private readonly Action<T?> sendResponse;

        /// <summary>
        /// Assign drag drop handlers to control.
        /// 
        /// When the drag start 'getData' is called to retrieve the data from the source.
        /// When the drop occurs 'sendData' is called on the destination.
        /// Any value returned from 'sendData' will be returned to the source with 'sendResponse'.
        /// </summary>
        /// <param Name="control"></param>
        /// <param Name="getData"></param>
        /// <param Name="sendData"></param>
        /// <param Name="sendResponse"></param>
        public ControlDragHandlers(Control control, Func<T?> getData, Func<T?, T?> sendData, Action<T?> sendResponse) {
            this.control = control;
            this.getData = getData;
            this.sendData = sendData;
            this.sendResponse = sendResponse;

            control.DragDrop += new DragEventHandler(this.OnDropStart);
            control.DragEnter += new DragEventHandler(this.OnDragEnter);
            control.MouseDown += new MouseEventHandler(this.OnDragStart);
        }

        public void OnDragStart(object? sender, MouseEventArgs? e) {
            if (e?.Button != MouseButtons.Left) return;

            var packet = new DataPacket<T> {
                data = this.getData()
            };

            this.control.DoDragDrop(packet, DragDropEffects.Move);
            this.sendResponse?.Invoke(packet.response);
        }

        private void OnDropStart(object? receiver, DragEventArgs e) {
            if (e?.Data == null) return;
            var data = e?.Data.GetData(typeof(DataPacket<T>));

            if (data is not DataPacket<T> packet) return; // BLOG
            packet.response = this.sendData(packet.data);
        }

        public void OnDragEnter(object? sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }
    }
}
