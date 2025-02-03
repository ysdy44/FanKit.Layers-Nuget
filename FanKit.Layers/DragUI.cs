using FanKit.Layers.Collections;
using FanKit.Layers.Core;
using FanKit.Layers.DragDrop;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Layers
{
    /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI']/*" />
    public class DragUI<T>
        where T : class, ILayerBase
    {
        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.GuideHeight']/*" />
        public double GuideHeight { get; set; } = 16;

        private readonly LogicalTreeList<T> LogicalTree;

        private readonly VisualTreeList<T> VisualTree;

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.DragUI']/*" />
        public DragUI(LogicalTreeList<T> logicalTree, VisualTreeList<T> visualTree)
        {
            this.LogicalTree = logicalTree;
            this.VisualTree = visualTree;
        }

        #region Drag&Drop

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.CacheDragOverGuide']/*" />
        public void CacheDragOverGuide(double width, double zoomFactorForDepth, ContainerSizeEventHandler containerSizeFromIndex)
        {
            foreach (T item in this.LogicalTree)
            {
                item.Settings.Guide = DraggingGuide.Empty;
            }

            double y1 = 0;
            double y2 = 0;

            for (int i = 0; i < this.VisualTree.Count; i++)
            {
                T item = this.VisualTree[i];

                y2 = y1 + containerSizeFromIndex(i);
                item.Settings.Guide = new DraggingGuide(i, y1, y2, item.Depth * zoomFactorForDepth, width, GuideHeight);
                y1 = y2;
            }

            DraggingGuide guide = DraggingGuide.Empty;

            foreach (T item in this.LogicalTree)
            {
                if (item.Settings.Guide.IsEmpty)
                    item.Settings.Guide = guide;
                else
                    guide = item.Settings.Guide;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.GetIndexerPoint']/*" />
        public DropIndexer GetIndexer(DragOverUIPoint point, DragSourceType sourceType)
        {
            return this.GetIndexer(point.PositionY + point.VerticalOffset - point.HeaderHeight, sourceType);
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.GetIndexerPositionY']/*" />
        public DropIndexer GetIndexer(double positionY, DragSourceType sourceType)
        {
            switch (this.LogicalTree.Count)
            {
                case 0:
                    switch (sourceType)
                    {
                        case DragSourceType.Others:
                            return DropIndexer.InsertAtBottom;
                        case DragSourceType.UnselectedItems:
                        case DragSourceType.SelectedItems:
                            return DropIndexer.Empty;
                        default:
                            return DropIndexer.Empty;
                    }
                case 1:
                    switch (sourceType)
                    {
                        case DragSourceType.Others:
                            ILayerBase single = this.LogicalTree.Single();

                            return positionY > single.Settings.Guide.CenterY ?
                                DropIndexer.InsertAtBottom :
                                DropIndexer.InsertAtTop;
                        case DragSourceType.UnselectedItems:
                        case DragSourceType.SelectedItems:
                            return DropIndexer.Empty;
                        default:
                            return DropIndexer.Empty;
                    }
                default:
                    // First
                    ILayerBase itemFirst = this.LogicalTree.First();
                    if (positionY <= itemFirst.Settings.Guide.Bottom)
                    {
                        return positionY > itemFirst.Settings.Guide.CenterY ?
                            DropIndexer.InsertBelow(0) :
                            DropIndexer.InsertAtTop;
                    }

                    int indexLast = this.LogicalTree.Count - 1;
                    for (int i = 1; i < indexLast; i++)
                    {
                        ILayerBase item = this.LogicalTree[i];

                        // Others
                        if (positionY < item.Settings.Guide.Top) continue;
                        else if (positionY > item.Settings.Guide.Bottom) continue;
                        else
                        {
                            return positionY < item.Settings.Guide.CenterY ?
                               DropIndexer.InsertAbove(i) :
                               this.LogicalTree[i + 1].Depth < item.Depth ?
                                   DropIndexer.InsertBelow(i) :
                                   DropIndexer.InsertAbove(i + 1);
                        }
                    }

                    // Last
                    ILayerBase itemLast = this.LogicalTree.Last();
                    return positionY > itemLast.Settings.Guide.Bottom ?
                        DropIndexer.InsertAtBottom :
                        positionY < itemLast.Settings.Guide.CenterY ?
                            DropIndexer.InsertAbove(indexLast) :
                            DropIndexer.InsertBelow(indexLast);
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.GetUIRect']/*" />
        public DragOverUIRect GetUIRect(DropIndexer Indexer)
        {
            switch (Indexer.Placement)
            {
                case DropPlacement.None:
                    return DragOverUIRect.Empty;
                case DropPlacement.InsertAtTop:
                    switch (this.LogicalTree.Count)
                    {
                        case 0:
                            return DragOverUIRect.Empty;
                        case 1:
                            return new DragOverUIRect(this.LogicalTree.Single().Settings.Guide.InsertAbove);
                        default:
                            return new DragOverUIRect(this.LogicalTree.First().Settings.Guide.InsertAbove);
                    }
                case DropPlacement.InsertAtBottom:
                    switch (this.LogicalTree.Count)
                    {
                        case 0:
                            return DragOverUIRect.Empty;
                        case 1:
                            return new DragOverUIRect(this.LogicalTree.Single().Settings.Guide.InsertAtBottom);
                        default:
                            return new DragOverUIRect(this.LogicalTree.Last().Settings.Guide.InsertAtBottom);
                    }
                case DropPlacement.InsertAbove:
                    return new DragOverUIRect(this.LogicalTree[Indexer.Index].Settings.Guide.InsertAbove);
                case DropPlacement.InsertBelow:
                    return new DragOverUIRect(this.LogicalTree[Indexer.Index].Settings.Guide.InsertBelow);
                default:
                    return DragOverUIRect.Empty;
            }
        }

        /// <include file="doc/docs.xml" path="docs/doc[@for='DragUI.GetUIRectPoint']/*" />
        public DragOverUIRect GetUIRect(DragOverUIPoint point, DropIndexer Indexer)
        {
            switch (Indexer.Placement)
            {
                case DropPlacement.None:
                    return DragOverUIRect.Empty;
                case DropPlacement.InsertAtTop:
                    switch (this.LogicalTree.Count)
                    {
                        case 0:
                            return DragOverUIRect.Empty;
                        case 1:
                            return new DragOverUIRect(point, this.LogicalTree.Single().Settings.Guide.InsertAbove);
                        default:
                            return new DragOverUIRect(point, this.LogicalTree.First().Settings.Guide.InsertAbove);
                    }
                case DropPlacement.InsertAtBottom:
                    switch (this.LogicalTree.Count)
                    {
                        case 0:
                            return DragOverUIRect.Empty;
                        case 1:
                            return new DragOverUIRect(point, this.LogicalTree.Single().Settings.Guide.InsertAtBottom);
                        default:
                            return new DragOverUIRect(point, this.LogicalTree.Last().Settings.Guide.InsertAtBottom);
                    }
                case DropPlacement.InsertAbove:
                    return new DragOverUIRect(point, this.LogicalTree[Indexer.Index].Settings.Guide.InsertAbove);
                case DropPlacement.InsertBelow:
                    return new DragOverUIRect(point, this.LogicalTree[Indexer.Index].Settings.Guide.InsertBelow);
                default:
                    return DragOverUIRect.Empty;
            }
        }

        #endregion
    }
}