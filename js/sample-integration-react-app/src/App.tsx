import React, { useEffect, useState } from 'react';
import { callHost } from './hostCall';
import { init, registerHostFunc } from './hostEvent';
import './App.css';

function App(): React.ReactElement {
  const [sliderValue, setSliderValue] = useState<number>(1);
  const [objectName, setObjectName] = useState<string>('');
  const [layerName, setLayerName] = useState<string>('');
  const [displayColor, setDisplayColor] = useState<string>('');

  useEffect(() => {
    // C#から任意のタイミングで実行できるように初期化
    init();
    
    // C#から呼び出し可能な関数を登録
    registerHostFunc("setRhinoProperty", setRhinoProperty);
    
    // 初期値でホストに通知
    callHost("UpdateObjectSize", {
      "size": sliderValue
    });
  }, []);

  useEffect(() => {
    // スライダーの値が変更されたときにC#に通知
    callHost("UpdateObjectSize", {
      "size": sliderValue
    });
  }, [sliderValue]);

  const setRhinoProperty = (name: string, layerName: string, color: string): void => {
    setObjectName(name);
    setLayerName(layerName);
    setDisplayColor(color);
  };

  const handleSliderChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const newValue = parseFloat(event.target.value);
    setSliderValue(newValue);
  };

  return (
    <div className="container">
      <h1>Rhino React UI</h1>
      <div className="rhinoProperty">
        <p>オブジェクト名: <strong>{objectName || 'なし'}</strong></p>
        <p>レイヤ名: <strong>{layerName || 'なし'}</strong></p>
        <p>表示色: <strong>{displayColor || 'なし'}</strong></p>
        <div
          className="colorBox"
          style={{
            backgroundColor: displayColor || '#ffffff',
          }}
        ></div>
      </div>
      <div className="slider-container">
        <label htmlFor="size-slider">オブジェクトのサイズ倍率:</label>
        <input
          id="size-slider"
          type="range"
          min="0.1"
          max="5"
          step="0.1"
          value={sliderValue}
          onChange={handleSliderChange}
        />
        <span className="slider-value">{sliderValue}</span>
      </div>
    </div>
  );
}

export default App;
