import { useRef, useState } from "react";
import { Scatter } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend, ChartOptions } from 'chart.js';
import { color } from "chart.js/helpers";

// Регистрация компонентов Chart.js
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

// interface DataPoint {
//   x: number;
//   y: number;
// }

// interface DataSet {
//   label: string;
//   data: DataPoint[];
//   backgroundColor: string;
// }

const data = {
  datasets: [
    {
      data: [{ x: 1, y: 1 }, { x: 2, y: 2 }],
      color: 'black',
    },
    {
      label: 'Cluster 2',
      data: [{ x: 1, y: 2 }, { x: 2, y: 1 }],
      backgroundColor: 'blue',
    }
  ]
};

const options: ChartOptions<'scatter'> = {
  scales: {
    x: {
      type: 'linear',
      position: 'bottom',
      min: -6,
      max: 8
    },
    y: {
      type: 'linear',
      min: -6,
      max: 5
    }
  }
};

export const ScatterPlot = ({setPLotData, setSlustersCount}) => {
  return <Scatter data={data} options={options} />;
}