using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using System;
using System.Collections.ObjectModel;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FanKit.Layers.TestApp
{
    public class VisualTreeNode : ITreeNode
    {
        //@Const
        private const double ZoomFactorForDepth = 16;

        public Guid Id { get; set; }
        public int Depth { get; set; }
        public bool IsExpanded { get; set; }
        public NodeSettings Settings { get; } = new NodeSettings();

        public Thickness DepthMargin => new Thickness(ZoomFactorForDepth * this.Depth, 0, 0, 0);
    }

    public class VisualTreeGroup
    {
        public VisualTreeList<VisualTreeNode> TreeList { get; set; }
    }

    public sealed partial class UISyncPage : Page
    {
        static readonly UISettings UISettings = new UISettings();

        readonly DispatcherTimer Timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000)
        };

        public ObservableCollection<VisualTreeNode> UINodes { get; } = new ObservableCollection<VisualTreeNode>();
        public VisualTreeGroup[] TreeGroups { get; } = new VisualTreeGroup[]
        {
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("556d137c-8fc8-4a8f-ab94-314807b85f66"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("9d56c297-4283-4f4c-b6f6-5fd979506df2"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("ffcbd663-ec9b-49ac-8076-2ac6c25ce71f"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("d7101556-80b9-416f-a277-9f42ca7b707e"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b0b05ce1-4096-4227-9a71-0645eff1e3b6"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("556d137c-8fc8-4a8f-ab94-314807b85f66"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("9d56c297-4283-4f4c-b6f6-5fd979506df2"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("ffcbd663-ec9b-49ac-8076-2ac6c25ce71f"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("6138f4f8-9a2f-4328-89c9-a752737183be"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("d7101556-80b9-416f-a277-9f42ca7b707e"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b0b05ce1-4096-4227-9a71-0645eff1e3b6"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("556d137c-8fc8-4a8f-ab94-314807b85f66"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("9d56c297-4283-4f4c-b6f6-5fd979506df2"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("ffcbd663-ec9b-49ac-8076-2ac6c25ce71f"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b13d1e92-f841-49b4-9fa9-5c7c53167618"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("6138f4f8-9a2f-4328-89c9-a752737183be"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("d7101556-80b9-416f-a277-9f42ca7b707e"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b0b05ce1-4096-4227-9a71-0645eff1e3b6"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("556d137c-8fc8-4a8f-ab94-314807b85f66"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("9d56c297-4283-4f4c-b6f6-5fd979506df2"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("ffcbd663-ec9b-49ac-8076-2ac6c25ce71f"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b13d1e92-f841-49b4-9fa9-5c7c53167618"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("6138f4f8-9a2f-4328-89c9-a752737183be"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b0b05ce1-4096-4227-9a71-0645eff1e3b6"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("556d137c-8fc8-4a8f-ab94-314807b85f66"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("9d56c297-4283-4f4c-b6f6-5fd979506df2"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("ffcbd663-ec9b-49ac-8076-2ac6c25ce71f"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("b13d1e92-f841-49b4-9fa9-5c7c53167618"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("6138f4f8-9a2f-4328-89c9-a752737183be"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("556d137c-8fc8-4a8f-ab94-314807b85f66"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("26d40f6f-8801-4610-930b-bbe913f25c33"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            },
            new VisualTreeGroup
            {
                TreeList = new VisualTreeList<VisualTreeNode>
                {
                    new VisualTreeNode { Id = new Guid("f9116f0d-0f7a-4e3b-8233-88b5d744facd"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("7fa255d9-fb81-4ba9-89ad-84c1dc48bba6"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("f9309be9-2784-408c-a923-faa4e9c5458b"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("be1f8ace-339a-4236-80d4-f4bd9a66221a"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("26d40f6f-8801-4610-930b-bbe913f25c33"), Depth = 0 },
                    new VisualTreeNode { Id = new Guid("997f6bfb-63d5-47df-9512-133be577d9f7"), Depth = 1 },
                    new VisualTreeNode { Id = new Guid("2667ee4d-c31f-4fe9-a84c-d77aa7aaab5c"), Depth = 2 },
                    new VisualTreeNode { Id = new Guid("3b37a1ae-2687-4e3d-8a23-04ef8d71590a"), Depth = 0 },
                }
            }
        };

        public UISyncPage()
        {
            this.InitializeComponent();
            base.Unloaded += (s, e) => this.Timer.Stop();
            base.Loaded += (s, e) => this.Timer.Start();

            this.SettingTextBlock.Text = $"[AnimationsEnabled: {UISettings.AnimationsEnabled}]";
            this.Timer.Tick += delegate
            {
                switch (this.TreeGroups.Length - this.UIListView.SelectedIndex)
                {
                    case 0:
                    case 1:
                        this.UIListView.SelectedIndex = 0;
                        break;
                    default:
                        this.UIListView.SelectedIndex++;
                        break;
                }

                VisualTreeGroup item = this.TreeGroups[this.UIListView.SelectedIndex];
                item.TreeList.UISyncTo(this.UINodes);
            };
            this.UIListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is VisualTreeGroup item)
                {
                    item.TreeList.UISyncTo(this.UINodes);
                }
            };
        }
    }
}