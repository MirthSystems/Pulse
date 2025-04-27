import React from 'react';
import { GraphData } from '../../pages/Profile';

interface ProfileDataProps {
  graphData: GraphData;
}

export const ProfileData: React.FC<ProfileDataProps> = ({ graphData }) => {
  return (
    <div id="profile-div">
      <h2>Profile Information</h2>
      <ul>
        <li><strong>Name: </strong> {graphData.displayName}</li>
        {graphData.jobTitle && <li><strong>Title: </strong> {graphData.jobTitle}</li>}
        {graphData.mail && <li><strong>Mail: </strong> {graphData.mail}</li>}
        {graphData.businessPhones && graphData.businessPhones.length > 0 && 
          <li><strong>Phone: </strong> {graphData.businessPhones[0]}</li>}
        {graphData.officeLocation && <li><strong>Location: </strong> {graphData.officeLocation}</li>}
      </ul>
    </div>
  );
};